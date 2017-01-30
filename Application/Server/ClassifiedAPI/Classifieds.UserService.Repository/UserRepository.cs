using Classifieds.UserService.BusinessEntities;
using MongoDB.Driver;
using System;
using System.Configuration;
using System.Linq;
using System.Net;

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
                
                returnStr = "Success";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnStr;
        }
        #endregion
    }
}
