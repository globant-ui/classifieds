using Classifieds.UserService.BusinessEntities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using System.Threading.Tasks;

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
                    var userResult = this.Classifieds.Save(user);
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
        /// <summary>
        /// Add user tags
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="tag"></param>
        public bool AddTag(string userEmail, Tags tag)
        { 
            try
            {
                var result= Classifieds.Update(Query.EQ("UserEmail", userEmail),
                Update.PushWrapped("Tags", tag));
                return result.UpdatedExisting;
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

        #endregion UpdateUserProfile

        #endregion
    }
}
