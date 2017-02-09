﻿using System;
using Classifieds.UserService.BusinessEntities;
using Classifieds.UserService.Repository;

namespace Classifieds.UserService.BusinessServices
{
    public class UserService : IUserService
    {
        #region Private Variables
        private readonly IUserRepository<ClassifiedsUser> _userRepository;
        private readonly ISubscriptionRepository<Subscription> _subRepository;
        #endregion

        #region Constructor
        public UserService(IUserRepository<ClassifiedsUser> userRepository, ISubscriptionRepository<Subscription> subRepository)
        {
            _userRepository = userRepository;
            _subRepository = subRepository;
        }

        #endregion

        #region Public Methods

        #region User Profile Methods
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
        public bool AddTag(string userEmail, Tags tag)
        {
            try
            {
              return _userRepository.AddTag(userEmail, tag);
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
        public bool DeleteTag(string userEmail, Tags tag)
        {
            try
            {
               return  _userRepository.DeleteTag(userEmail, tag);
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
        public bool AddAlert(string userEmail, Alert alert)
        {
            try
            {
                return _userRepository.AddAlert(userEmail, alert);
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
        public bool DeleteAlert(string userEmail, Alert alert)
        {
            try
            {
              return  _userRepository.DeleteAlert(userEmail, alert);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AddSubscription

        /// <summary>
        /// Insert new Subscription item into the database
        /// </summary>
        /// <param name="subscriptionObj">Subscription Object</param>
        /// <returns>Newly added Subscription object</returns>
        public Subscription AddSubscription(Subscription subscriptionObj)
        {
            try
            {
                return _subRepository.AddSubscription(subscriptionObj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DeleteSubscription
        /// <summary>
        /// Delete Subscription item for given Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>deleted Id</returns>
        public void DeleteSubscription(string id)
        {
            try
            {
                _subRepository.DeleteSubscription(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion DeleteSubscription

        #endregion
    }
}
