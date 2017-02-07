using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Net;
using System.Net.Http;
using Classifieds.Common.Entities;
using MongoDB.Driver;

namespace Classifieds.Common.Repositories
{
    public class CommonRepository : CommonDBRepository, ICommonRepository
    {
        #region Private variables
        private readonly string _collectionTokens = ConfigurationManager.AppSettings["UserTokensCollection"];
        private readonly ICommonDBRepository _dbRepository;
        private readonly ILogger _logger;
        private string _userEmail = string.Empty;
        #endregion
        MongoCollection<UserToken> UserTokens
        {
            get { return _dbRepository.GetCollection<UserToken>(_collectionTokens); }
        }

        #region Constructor
        public CommonRepository(ICommonDBRepository dbRepository, ILogger logger)
        {
            _dbRepository = dbRepository;
            _logger = logger;
        }
        #endregion

        #region public methods
        /// <summary>
        /// Checks whether the request is valid using token info in request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string IsAuthenticated(HttpRequestMessage request)
        {
            string result = string.Empty;
            try
            {
                IEnumerable<string> headerValues;
                HttpRequestMessage message = request ?? new HttpRequestMessage();
                if (!message.Headers.TryGetValues("AccessToken", out headerValues))
                    result = new HttpResponseMessage(HttpStatusCode.Unauthorized).ToString() + " Invalid Request";
                string accesstoken = headerValues.FirstOrDefault();

                if (!message.Headers.TryGetValues("UserEmail", out headerValues))
                    result = new HttpResponseMessage(HttpStatusCode.Unauthorized).ToString() + " Invalid Request";
                _userEmail = headerValues.FirstOrDefault();

                if (ValidateRequest(accesstoken, _userEmail))
                {
                    result = "200"; 
                }
                else
                {
                    result = new HttpResponseMessage(HttpStatusCode.Unauthorized).ToString() + " access denied";
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                result = new HttpResponseMessage(HttpStatusCode.Conflict).ToString() + " Pls try after some time.";
            }
            return result;
        }

        /// <summary>
        /// Saves a new user token into the database
        /// </summary>
        /// <param name="userToken">UserToken object</param>
        /// <returns>return token</returns>
        public UserToken SaveToken(UserToken userToken)
        {
            try
            {
                UserTokens.Save(userToken);
                return userToken;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, userToken.UserEmail);
                throw ex;
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Validates the token and email against db values
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        private bool ValidateRequest(string accessToken, string userEmail)
        {
            try
            {

                var tokens = this.UserTokens.FindAll()
                    .Where(p => p.AccessToken == accessToken && p.UserEmail == userEmail)
                    .ToList();
                return tokens.Count == 1 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, userEmail);
                throw ex;
            }
        }
        #endregion
    }
}
