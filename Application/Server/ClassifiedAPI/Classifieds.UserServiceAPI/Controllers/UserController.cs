using Classifieds.Common;
using Classifieds.UserService.BusinessEntities;
using Classifieds.UserService.BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ICommonDBRepository _commonRepository;
        #endregion

        #region Constructor
        /// <summary>
        /// The class constructor. 
        /// </summary>
        /// 
        public UserController()
        {
       
        }
        public UserController(IUserService userService, ILogger logger, ICommonDBRepository commonRepository)
        {
            _userService = userService;
            _logger = logger;
            _commonRepository = commonRepository;
        }
        #endregion

        #region Public Methods
        //[BasicAuthorization(true), HttpGet]
        public string Get()
        {
            return _commonRepository.IsAuthenticated(Request);
            //return "hi classifieds";
        }

        [HttpPost]
        public HttpResponseMessage Register(ClassifiedsUser user)
        {
            try
            {
                HttpResponseMessage response = null;
                if(user == null)
                    throw new Exception(HttpStatusCode.PreconditionFailed.ToString() + "Invalid request");
                else if(user.UserEmail == null || user.UserName == null)
                    throw new Exception(HttpStatusCode.PreconditionFailed.ToString() + "Invalid request");

                var result = _userService.RegisterUser(user);

                //saving auth token
                if (result.Equals("Success") || result.Equals("Saved"))
                {
                    var TokenId = Guid.NewGuid().ToString("n");
                    UserToken userToken = new UserToken();
                    userToken.AccessToken = TokenId;
                    userToken.UserEmail = user.UserEmail;
                    userToken.LoginDateTime = DateTime.Now.ToString();
                    _userService.SaveToken(userToken);
                    response = Request.CreateResponse<UserToken>(HttpStatusCode.Created, userToken);
                }
                return response;
            }
            catch (Exception ex)
            {
                if (user != null)
                {
                    if (user.UserEmail != null)
                        _logger.Log(ex, user.UserEmail);
                    else
                        _logger.Log(ex, string.Empty);
                }
                else
                    _logger.Log(ex, string.Empty);
                throw new Exception(HttpStatusCode.Conflict.ToString() + "Internal server error");
            }
         }
        #endregion
    }
}
