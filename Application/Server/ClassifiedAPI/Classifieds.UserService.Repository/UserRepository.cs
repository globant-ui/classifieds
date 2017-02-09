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
                return  _dbRepository.GetCollection<TEntity>(_collectionClassifieds);
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
                    //.Set(p => p.UserEmail, userProfile.UserEmail)
                    .Set(p => p.Designation, userProfile.Designation)
                    .Set(p => p.Image, userProfile.Image)
                    .Set(p => p.Location, userProfile.Location)
                    .Set(p => p.UserName, userProfile.UserName)
                    .Set(p => p.Mobile, userProfile.Mobile);
                    //.Set(p => p.WishList, userProfile.WishList)
                    //.Set(p => p.Alert[0], userProfile.Alert[0])
                    //.Set(p => p.Tags[0], userProfile.Tags[0]);
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
        public void AddTag(string userEmail, Tags tag)
        { 
            try
            {
                var result= Classifieds.Update(Query.EQ("UserEmail", userEmail),
                Update.PushWrapped("Tags", tag));
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
        public void DeleteTag(string userEmail, Tags tag)
        {
            try
            {
                var result = Classifieds.Update(Query.EQ("UserEmail", userEmail),
                Update.PullWrapped<Tags>("Tags", tag));
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
        public void AddAlert(string userEmail, Alert alert)
        {
            try
            {
                var result = Classifieds.Update(Query.EQ("UserEmail", userEmail),
                Update.PushWrapped("Alert", alert));
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
        public void DeleteAlert(string userEmail, Alert alert)
        {
            try
            {
                var result = Classifieds.Update(Query.EQ("UserEmail", userEmail),
                Update.PullWrapped<Alert>("Alert", alert));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
