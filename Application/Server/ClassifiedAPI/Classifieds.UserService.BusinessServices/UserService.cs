using System;
using Classifieds.UserService.BusinessEntities;
using Classifieds.UserService.Repository;

namespace Classifieds.UserService.BusinessServices
{
    public class UserService : IUserService
    {
        #region Private Variables
        private readonly IUserRepository _userRepository;
        #endregion

        #region Constructor
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Registers a classifieds user into the database
        /// </summary>
        /// <param name="user">ClassifiedsUser Object</param>
        /// <returns></returns>
        public string RegisterUser(ClassifiedsUser user)
        {
            try
            {
                return _userRepository.RegisterUser(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
