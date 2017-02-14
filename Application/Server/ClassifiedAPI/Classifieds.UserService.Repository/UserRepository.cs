using Classifieds.UserService.BusinessEntities;
using MongoDB.Driver;
using System;
using System.Configuration;
using System.Linq;
using MongoDB.Driver.Builders;

namespace Classifieds.UserService.Repository
{
    public class UserRepository<TEntity> : DBRepository, IUserRepository<TEntity> where TEntity : ClassifiedsUser
    {
        #region Private Variables
        private readonly string _collectionClassifieds = ConfigurationManager.AppSettings["UserCollection"];
        private readonly IDBRepository _dbRepository;
        MongoCollection<TEntity> Classifieds
        {
            get
            {
                return _dbRepository.GetCollection<TEntity>(_collectionClassifieds);
            }
        }
        #endregion

        #region Constructor
        public UserRepository(IDBRepository dBRepository)
        {
            _dbRepository = dBRepository;
        }
        #endregion

        #region Public Methods

        #region RegisterUser

        /// <summary>
        /// Insert a new user object into the database
        /// </summary>
        /// <param name="user">ClassifiedsUser object</param>
        /// <returns>return newly added listing object</returns>
        public string RegisterUser(TEntity user)
        {
            string returnStr = string.Empty;
            try
            {
                var result = Classifieds.FindAll()
                                .Where(p => p.UserEmail == user.UserEmail)
                                .ToList();
                if (result.Count == 0)
                {
                    var userResult = Classifieds.Save(user);
                    if (userResult.DocumentsAffected == 0 && userResult.HasLastErrorMessage)
                    {
                        throw new Exception("Registrtion failed");
                    }
                    returnStr = "Saved";
                }
                else
                {
                    returnStr = "Success";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnStr;
        }

        #endregion RegisterUser

        #region GetUserProfile

        /// <summary>
        /// Get complete userprofile including tags, subscriptions, wishlist
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns>userProfile object</returns>
        public TEntity GetUserProfile(string userEmail)
        {
            try
            {
                var result = Classifieds.FindAll()
                                   .Where(p => p.UserEmail == userEmail)
                                   .ToList();
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion GetUserProfile

        #region UpdateUserProfile

        /// <summary>
        /// update user profile
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns>updated object of ClassifiedsUser</returns>
        public TEntity UpdateUserProfile(TEntity userProfile)
        {
            try
            {
                var query = Query<ClassifiedsUser>.EQ(p => p._id, userProfile._id);
                var update = Update<ClassifiedsUser>
                    .Set(p => p.Designation, userProfile.Designation)
                    .Set(p => p.Image, userProfile.Image)
                    .Set(p => p.Location, userProfile.Location)
                    .Set(p => p.UserName, userProfile.UserName)
                    .Set(p => p.Mobile, userProfile.Mobile);
                var result = Classifieds.Update(query, update);
                return userProfile;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion UpdateUserProfile

        #region User Tag Add, Delete
        /// <summary>
        /// Add user tags
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="tags"></param>
        public bool AddTag(string userEmail, Tags[] tags)
        { 
            try
            {
                bool result=true;
                foreach (var tag in tags)
                {
                    var returnResult = Classifieds.Update(Query.EQ("UserEmail", userEmail),
                    Update.PushWrapped("Tags", tag));
                    result = returnResult.UpdatedExisting;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        /// <summary>
        /// Delete tags
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="tag"></param>
        public bool DeleteTag(string userEmail, Tags tag)
        {
            try
            {
                var result = Classifieds.Update(Query.EQ("UserEmail", userEmail),
                Update.PullWrapped<Tags>("Tags", tag));
                return result.UpdatedExisting;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region User Alert Add, Delete
        /// <summary>
        /// Add Alerts
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="alert"></param>
        public bool AddAlert(string userEmail, Alert alert)
        {
            try
            {
                var result = Classifieds.Update(Query.EQ("UserEmail", userEmail),
                Update.PushWrapped("Alert", alert));
                return result.UpdatedExisting;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Delete Alerts
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="alert"></param>
        public bool DeleteAlert(string userEmail, Alert alert)
        {
            try
            {
                var result = Classifieds.Update(Query.EQ("UserEmail", userEmail),
                Update.PullWrapped<Alert>("Alert", alert));
                return result.UpdatedExisting;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region User WishList Get, Add and Delete
        /// <summary>
        /// Add to wishList
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="listingId"></param>
        public bool AddtoWishList(string userEmail, string listingId)
        {
            try
            {
                var result = Classifieds.Update(Query.EQ("UserEmail", userEmail),
                Update.PushWrapped("WishList", listingId));
                return result.UpdatedExisting;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="listingId"></param>
        /// <returns></returns>
        public bool DeleteFromWishList(string userEmail, string listingId)
        {
            try
            {
                var result = Classifieds.Update(Query.EQ("UserEmail", listingId),
                Update.PullWrapped("WishList", listingId));
                return result.UpdatedExisting;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get UserWishList
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns>string array of Listing id's</returns>
        public string[] GetUserWishList(string userEmail)
        {
            try
            {
                var query = Query.EQ("UserEmail", userEmail);
                var items = Classifieds.Find(query).ToList();
                return items[0].WishList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get user Recommonded TagList
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns>returns taglist</returns>
        public Tags[] GetRecommondedTagList(string userEmail)
        {
            try
            {
                var query = Query.EQ("UserEmail", userEmail);
                var items = Classifieds.Find(query).ToList();
                return items[0].Tags;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #endregion
    }
}
