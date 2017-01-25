using Classifieds.UserService.BusinessEntities;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Configuration;
using System.Linq;

namespace Classifieds.UserService.Repository
{
    public class UserRepository:DBRepository, IUserRepository
    {
        #region Private Variables
        private readonly string _collectionClassifieds = ConfigurationManager.AppSettings["UserCollection"];
        private readonly IDBRepository _dbRepository;
        MongoCollection<ClassifiedsUser> _classifieds
        {
            get { return _dbRepository.GetCollection<ClassifiedsUser>(_collectionClassifieds); }
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
            try
            {
                var result = this._classifieds.FindAll()
                                .Where(p => p.UserEmail == user.UserEmail)
                                .ToList();
                if (result.Count == 0)
                {
                    var userResult = this._classifieds.Save(user);
                    if (userResult.DocumentsAffected == 0 && userResult.HasLastErrorMessage) { }
                    return "Saved";
                }
                
                return "Success";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
