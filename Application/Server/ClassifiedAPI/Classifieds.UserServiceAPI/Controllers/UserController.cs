using Classifieds.Common;
using Classifieds.Common.Repositories;
using Classifieds.UserService.BusinessEntities;
using Classifieds.UserService.BusinessServices;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using System.IO;
using System.Net.Http.Headers;

namespace Classifieds.UserServiceAPI.Controllers
{
    /// <summary>
    /// This Service is used to perform user operations
    /// class name: UserController
    /// Purpose : This class is used to implement post/put methods on users
    /// Created By : Ashish
    /// Created Date: 10/01/2017
    /// Modified by :Amol Pawar
    /// Modified date: 
    /// </summary>
    public class UserController : ApiController
    {
        #region Private Variable
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private readonly ICommonRepository _commonRepository;
        private string _userEmail = string.Empty;
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

        #region Get Methods

        #region GetUserProfile
        /// <summary>
        /// Get user profile including user tags, wish list, Alert.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns>Response containing user details json object</returns>
        [HttpGet]
        public ClassifiedsUser GetUserProfile(string userEmail)
        {
            try
            {
                if (userEmail == null)
                {
                    throw new ArgumentNullException();
                }
                _userEmail = GetUserEmailFromHeader();
                string authResult = _commonRepository.IsAuthenticated(Request);
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                ClassifiedsUser objUser = new ClassifiedsUser();
                objUser = _userService.GetUserProfile(userEmail);
                if (!String.IsNullOrEmpty(objUser.Image))
                {
                    objUser.Image = ConfigurationManager.AppSettings["ImageServer"].ToString() + objUser.Image;
                }
                return objUser;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }
        #endregion

        #region GetUserWishList
        /// <summary>
        /// Get user wishlist
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns>String Array of Listing Ids</returns>
        public string[] GetUserWishList(string userEmail)
        {
            try
            {
                _userEmail = GetUserEmailFromHeader();
                string authResult = _commonRepository.IsAuthenticated(Request);
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                return _userService.GetUserWishList(userEmail);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }
        #endregion

        #region GetRecommondedTagList
        /// <summary>
        /// Add user recommonded TagList
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns>user Tag object</returns>
        public Tags GetRecommondedTagList(string userEmail)
        {
            try
            {
                _userEmail = GetUserEmailFromHeader();
                string authResult = _commonRepository.IsAuthenticated(Request);
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                return _userService.GetRecommondedTagList(userEmail);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }
        #endregion

        #endregion

        #region Post, Put, Delete

        #region RegisterUser
        /// <summary>
        /// Registers the user if not present in Db
        /// </summary>
        /// <param name="user">User object</param>
        /// <returns>Response containing access token</returns>
        [HttpPost]
        public HttpResponseMessage RegisterUser(ClassifiedsUser user)
        {
            string email = string.Empty;
            try
            {
                email = GetUserEmail(user);
                HttpResponseMessage response = null;
                if (user == null || user.UserEmail == null || user.UserName == null)
                    throw new Exception(HttpStatusCode.PreconditionFailed.ToString() + "Invalid request");
                else if (!(user.UserEmail.ToLowerInvariant().EndsWith("globant.com")))
                    throw new Exception(HttpStatusCode.PreconditionFailed.ToString() + "Invalid domain");

                var result = _userService.RegisterUser(user);

                //saving auth token
                if (result.Equals("Success") || result.Equals("Saved"))
                {
                    var tokenId = Guid.NewGuid().ToString("n");
                    Classifieds.Common.Entities.UserToken userToken = new Classifieds.Common.Entities.UserToken();
                    userToken.AccessToken = tokenId;
                    userToken.UserEmail = user.UserEmail;
                    userToken.LoginDateTime = DateTime.Now;
                    userToken.IsFirstTimeLogin = result == "Saved" ? true : false;
                    _commonRepository.SaveToken(userToken);
                    response = Request.CreateResponse<Classifieds.Common.Entities.UserToken>(HttpStatusCode.Created, userToken);
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, email);
                throw new Exception(HttpStatusCode.Conflict.ToString() + " Internal server error");
            }
        }
        #endregion

        #region UpdateUserProfile
        /// <summary>
        /// update user profile
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns>updated user details</returns>
        [HttpPut]
        public ClassifiedsUser UpdateUserProfile(ClassifiedsUser userProfile)
        {
            try
            {
                if (userProfile == null)
                {
                    throw new ArgumentNullException();
                }
                _userEmail = GetUserEmailFromHeader();
                string authResult = _commonRepository.IsAuthenticated(Request);
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                return _userService.UpdateUserProfile(userProfile);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }
        #endregion

        #region AddTag
        /// <summary>
        /// Add user tag by subcategory and Locations and update user Profile
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="tag"></param>
        /// <returns>boolen true as success</returns>
        [HttpPost]
        public Tags AddTag(string userEmail, Tags tag)
        {
            try
            {
                if (userEmail == null)
                {
                    throw new ArgumentNullException();
                }
                _userEmail = GetUserEmailFromHeader();
                string authResult = _commonRepository.IsAuthenticated(Request);
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                return _userService.AddTag(userEmail, tag);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }
        #endregion

        #region DeleteTag
        /// <summary>
        /// Delete User Tag and update user profile
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="tagName"></param>
        /// <returns>boolen true as success</returns>
        [HttpDelete]
        public bool DeleteTag(string userEmail, string tagName)
        {
            try
            {
                _userEmail = GetUserEmailFromHeader();
                string authResult = _commonRepository.IsAuthenticated(Request);
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                return _userService.DeleteTag(userEmail, tagName);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }
        #endregion

        #region AddAlert
        /// <summary>
        /// Add user Alerts by subcategory and Locations, update user profile
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="alert"></param>
        /// <returns>boolen true as success</returns>
        [HttpPost]
        public bool AddAlert(string userEmail, Alert alert)
        {
            try
            {
                _userEmail = GetUserEmailFromHeader();
                string authResult = _commonRepository.IsAuthenticated(Request);
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                return _userService.AddAlert(userEmail, alert);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }
        #endregion

        #region DeleteAlerts
        /// <summary>
        /// Removes an alert from the Alert array object in User
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="alert"></param>
        /// <returns></returns>
        [HttpPut]
        public bool DeleteAlerts(string userEmail, Alert alert)
        {
            try
            {
                _userEmail = GetUserEmailFromHeader();
                string authResult = _commonRepository.IsAuthenticated(Request);
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                return _userService.DeleteAlert(userEmail, alert);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }
        #endregion

        #region AddToWishList
        /// <summary>
        /// Add to wishList update UserProfile
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="listingId"></param>
        /// <returns>boolen true as success</returns>
        [HttpPost]
        public bool AddToWishList(string userEmail, string listingId)
        {
            try
            {
                _userEmail = GetUserEmailFromHeader();
                string authResult = _commonRepository.IsAuthenticated(Request);
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                return _userService.AddtoWishList(userEmail, listingId);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }
        #endregion

        #region DeleteFromWishList
        /// <summary>
        /// Delete listing id from wishList and update UserProfile
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="listingId"></param>
        /// <returns>boolen true as success</returns>
        [HttpDelete]
        public bool DeleteFromWishList(string userEmail, string listingId)
        {
            try
            {
                _userEmail = GetUserEmailFromHeader();
                string authResult = _commonRepository.IsAuthenticated(Request);
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                return _userService.DeleteFromWishList(userEmail, listingId);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }
        #endregion

        #region PostUserImage
        /// <summary>
        /// Post User profile Image
        /// </summary>
        /// <returns>Task</returns>
        public async Task<HttpResponseMessage> PostUserImage()
        {
            string resPath = string.Empty;
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = GetUserEmailFromHeader();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                Dictionary<string, object> dict = new Dictionary<string, object>();
                var httpRequest = HttpContext.Current.Request;
                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int imageSize = Convert.ToInt32(ConfigurationManager.AppSettings["ImageSize"]);
                        int maxContentLength = 1024 * 1024 * imageSize; //Size = 1 MB  

                        IList<string> allowedFileExtensions = new List<string>();
                        string[] imgExt = ConfigurationManager.AppSettings["ImageExtList"].Split(',');
                        allowedFileExtensions = imgExt.ToList();

                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!allowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload image of type " + ConfigurationManager.AppSettings["ImageExtList"].ToString());
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > maxContentLength)
                        {
                            var message = string.Format("Please Upload a file upto " + ConfigurationManager.AppSettings["ImageSize"] + " MB");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            string[] loginNameParts = _userEmail.Split('@');
                            string userName = loginNameParts[0];
                            string path = HttpContext.Current.Server.MapPath("~/");
                            path = path + ConfigurationManager.AppSettings["BasePathProfileImage"].ToString();
                            path = path + userName + @"\";
                            if (!System.IO.Directory.Exists(path))
                            {
                                System.IO.Directory.CreateDirectory(path);
                            }
                            string uniqueUserName = userName + Guid.NewGuid().ToString();
                            postedFile.SaveAs(path + uniqueUserName + extension);
                            resPath = ConfigurationManager.AppSettings["DBSaveProfileImage"].ToString() + userName + "/" + uniqueUserName + extension;
                            _userService.UpdateImagePath(_userEmail, resPath);
                        }
                    }
                    //return image path in response
                    return Request.CreateErrorResponse(HttpStatusCode.Created, ConfigurationManager.AppSettings["ImageServer"].ToString() + resPath); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw;
            }
        }
        #endregion
        #endregion

        #endregion

        #region Private methods
        /// <summary>
        /// Returns user email string
        /// </summary>
        /// <param name="user">ClassifiedsUser object</param>
        /// <returns>string</returns>
        private string GetUserEmail(ClassifiedsUser user)
        {
            string result = string.Empty;
            if (user?.UserEmail != null)
            {
                result = user.UserEmail;
            }
            return result;
        }
        /// <summary>
        /// Returns user email string for header
        /// </summary>
        /// <returns>string</returns>
        private string GetUserEmailFromHeader()
        {
            IEnumerable<string> headerValues;
            HttpRequestMessage message = Request ?? new HttpRequestMessage();
            message.Headers.TryGetValues("UserEmail", out headerValues);
            string hearderVal = headerValues == null ? string.Empty : headerValues.FirstOrDefault();
            return hearderVal;
        }
        #endregion
    }
}
