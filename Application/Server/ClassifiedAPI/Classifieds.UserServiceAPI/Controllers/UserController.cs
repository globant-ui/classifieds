﻿using Classifieds.Common;
using Classifieds.Common.Repositories;
using Classifieds.UserService.BusinessEntities;
using Classifieds.UserService.BusinessServices;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Classifieds.UserServiceAPI.Controllers
{
    /// <summary>
    /// This Service is used to perform user operations
    /// class name: UserController
    /// Purpose : This class is used to implement post/put methods on users
    /// Created By : Ashish
    /// Created Date: 10/01/2017
    /// Modified by :
    /// Modified date: 
    /// </summary>
    [EnableCors("http://localhost:3000", "*", "*")]
    public class UserController : ApiController
    {
        #region Private Variable
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private readonly ICommonRepository _commonRepository;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor with injected dependencies
        /// </summary>
        public UserController(IUserService userService, ILogger logger, ICommonRepository commonRepository)
        {
            _userService = userService;
            _logger = logger;
            _commonRepository = commonRepository;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Registers the user if not present in Db
        /// </summary>
        /// <param name="user">User object</param>
        /// <returns>Response containing access token</returns>
        [HttpPost]
        public HttpResponseMessage RegisterUser(ClassifiedsUser user)
        {
            try
            {
                HttpResponseMessage response = null;
                if(user == null)
                    throw new Exception(HttpStatusCode.PreconditionFailed.ToString() + "Invalid request");
                else if(user.UserEmail == null || user.UserName == null)
                    throw new Exception(HttpStatusCode.PreconditionFailed.ToString() + "Invalid request");
                else if(!(user.UserEmail.ToLowerInvariant().EndsWith("globant.com")))
                    throw new Exception(HttpStatusCode.PreconditionFailed.ToString() + "Invalid domain");

                var result = _userService.RegisterUser(user);

                //saving auth token
                if (result.Equals("Success") || result.Equals("Saved"))
                {
                    var tokenId = Guid.NewGuid().ToString("n");
                    Classifieds.Common.Entities.UserToken userToken = new Classifieds.Common.Entities.UserToken();
                    userToken.AccessToken = tokenId;
                    userToken.UserEmail = user.UserEmail;
                    userToken.LoginDateTime = DateTime.Now.ToString();
                    _commonRepository.SaveToken(userToken);
                    response = Request.CreateResponse<Classifieds.Common.Entities.UserToken>(HttpStatusCode.Created, userToken);
                }
                return response;
            }
            catch (Exception ex)
            {
                string email = getUserEmail(user);
                _logger.Log(ex, email);
                throw new Exception(HttpStatusCode.Conflict.ToString() + "Internal server error");
            }
         }
        #endregion
        #region private methods
        /// <summary>
        /// Returns user email string
        /// </summary>
        /// <param name="user">ClassifiedsUser object</param>
        /// <returns>string</returns>
        private string getUserEmail(ClassifiedsUser user)
        {
            if (user != null)
            {
                if (user.UserEmail != null)
                    return user.UserEmail;
                else
                    return string.Empty;
            }
            else
                return string.Empty;
        }

        #endregion
    }
}
