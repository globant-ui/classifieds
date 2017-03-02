using System;
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
                if (user.Tags == null)
                {
                    Tags objTag = new Tags();
                    objTag.SubCategory = new string[] {};
                    objTag.Location = new string[] {};
                    user.Tags = objTag;
                }
                if (user.Alert == null)
                {
                    Alert[] objAlert= new Alert[] {};
                    user.Alert = objAlert;
                }
                if (user.WishList == null)
                {
                    user.WishList= new string[] {};
                }
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
        /// <returns>user tag object</returns>
        public Tags AddTag(string userEmail, Tags tag)
        {
            try
            {
               _userRepository.AddTag(userEmail, tag);
                var result = _userRepository.GetUserProfile(userEmail);
                return result.Tags;
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
        /// <param name="tagName"></param>
        public bool DeleteTag(string userEmail, string tagName)
        {
            try
            {
               return  _userRepository.DeleteTag(userEmail, tagName);
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
        /// <summary>
        /// Add listing id in to wish List and update User profile.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="listingId"></param>
        /// <returns></returns>
        public bool AddtoWishList(string userEmail, string listingId)
        {
            try
            {
                return _userRepository.AddtoWishList(userEmail, listingId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Delete listing id from WishList and update user profile
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="listingId"></param>
        /// <returns></returns>
        public bool DeleteFromWishList(string userEmail, string listingId)
        {
            try
            {
                return _userRepository.DeleteFromWishList(userEmail, listingId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string[] GetUserWishList(string userEmail)
        {
            try
            {
                return _userRepository.GetUserWishList(userEmail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Tags GetRecommondedTagList(string userEmail)
        {
            try
            {
                return _userRepository.GetRecommondedTagList(userEmail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Update User Image path
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="imgPath"></param>
        public void UpdateImagePath(string userEmail, string imgPath)
        {
            try
            {
                 _userRepository.UpdateImagePath(userEmail, imgPath);
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
