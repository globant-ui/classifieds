using System;
using Classifieds.UserService.BusinessEntities;
using Classifieds.UserService.Repository;

namespace Classifieds.UserService.BusinessServices
{
    public class UserService : IUserService
    {
        #region Private Variables
        private readonly IUserRepository<ClassifiedsUser> _userRepository;
        #endregion

        #region Constructor
        public UserService(IUserRepository<ClassifiedsUser> userRepository)
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
        /// <summary>
        /// Get all user profile details
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns>user detail object</returns>
        public ClassifiedsUser GetUserProfile(string userEmail)
        {
            try
            {
                return _userRepository.GetUserProfile(userEmail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Updates user profile.
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns>user profile object</returns>
        public ClassifiedsUser UpdateUserProfile(ClassifiedsUser userProfile)
        {
            try
            {
                return _userRepository.UpdateUserProfile(userProfile);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Add user tag
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="tag"></param>
        public void AddTag(string userEmail, Tags tag)
        {
            try
            {
               _userRepository.AddTag(userEmail, tag);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Delete tag of user profile
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="tag"></param>
        public void DeleteTag(string userEmail, Tags tag)
        {
            try
            {
                _userRepository.DeleteTag(userEmail, tag);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Add alert for user
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="alert"></param>
        public void AddAlert(string userEmail, Alert alert)
        {
            try
            {
                _userRepository.AddAlert(userEmail, alert);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Add alert for user
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="alert"></param>
        public void DeleteAlert(string userEmail, Alert alert)
        {
            try
            {
                _userRepository.DeleteAlert(userEmail, alert);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
