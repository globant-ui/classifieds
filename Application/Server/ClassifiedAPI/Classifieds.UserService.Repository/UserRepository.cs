using Classifieds.UserService.BusinessEntities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;

namespace Classifieds.UserService.Repository
{
    public class UserRepository:DBRepository, IUserRepository
    {
        #region Private Variables
        private readonly string _collectionClassifieds = ConfigurationManager.AppSettings["UserCollection"];
        private readonly IDBRepository _dbRepository;
        MongoCollection<ClassifiedsUser> Classifieds
        {
            get
            {
                return _dbRepository.GetCollection<ClassifiedsUser>(_collectionClassifieds);
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
        public string RegisterUser(ClassifiedsUser user)
        {
            string returnStr = string.Empty;
            try
            {
                var result = this.Classifieds.FindAll()
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
        public ClassifiedsUser GetUserProfile(string userEmail)
        {
            try
            {
                var result = this.Classifieds.FindAll()
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
        /// <param name="id"></param>
        /// <param name="userProfile"></param>
        /// <returns>updated object of ClassifiedsUser</returns>
        public ClassifiedsUser UpdateUserProfile(string id, ClassifiedsUser userProfile)
        {
            try
            {
                var query = Query<ClassifiedsUser>.EQ(p => p._id, id);
                var update = Update<ClassifiedsUser>.Set(p => p.UserEmail, userProfile.UserEmail)
                    .Set(p => p.Designation, userProfile.Designation)
                    .Set(p => p.Image, userProfile.Image)
                    .Set(p => p.Location, userProfile.Location)
                    .Set(p => p.UserName, userProfile.UserName)
                    .Set(p => p.Mobile, userProfile.Mobile)
                    .Set(p => p.WishList, userProfile.WishList)
                    .Set(p => p.Subscription[0], userProfile.Subscription[0])
                    .Set(p => p.Tags[0], userProfile.Tags[0]);
                var result = Classifieds.Update(query, update);
                if (result.DocumentsAffected == 0 && result.HasLastErrorMessage)
                {
                }
                return userProfile;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
    }
}
