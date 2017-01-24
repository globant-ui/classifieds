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
        #endregion
        MongoCollection<UserToken> UserTokens
        {
            get { return _dbRepository.GetCollection<UserToken>(_collectionTokens); }
        }

        #region Constructor
        public CommonRepository(ICommonDBRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        #endregion

        #region public methods
        public string IsAuthenticated(HttpRequestMessage request)
        {
            try
            {
                IEnumerable<string> headerValues;
                HttpRequestMessage message = request ?? new HttpRequestMessage();
                if (!message.Headers.TryGetValues("AccessToken", out headerValues))
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized).ToString() + " Invalid Request";
                string accesstoken = headerValues.FirstOrDefault();

                if (!message.Headers.TryGetValues("UserEmail", out headerValues))
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized).ToString() + " Invalid Request";
                string userEmail = headerValues.FirstOrDefault();

                if (this.validateRequest(accesstoken, userEmail))
                {
                    return "200"; // new HttpResponseMessage(HttpStatusCode.OK).ToString();
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized).ToString() + " access denied";
                }
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.Conflict).ToString() + "Pls try after some time.";
            }
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
                var result = this.UserTokens.Save(userToken);
                if (result.DocumentsAffected == 0 && result.HasLastErrorMessage) { }
                return userToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region private methods
        private bool validateRequest(string accessToken, string userEmail)
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
                return false;
            }
        }
        #endregion
    }
}
