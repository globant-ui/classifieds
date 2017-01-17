using System;
using System.Collections.Generic;
using System.Linq;
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
        public UserService(IUserRepository UserRepository)
        {
            _userRepository = UserRepository;
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

        /// <summary>
        /// Saves a user token into the database
        /// </summary>
        /// <param name="user">UserToken Object</param>
        /// <returns></returns>
        public UserToken SaveToken(UserToken userToken)
        {
            try
            {
                return _userRepository.SaveToken(userToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
