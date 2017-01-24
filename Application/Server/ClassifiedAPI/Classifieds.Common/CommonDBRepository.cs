using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Classifieds.Common.Entities;

namespace Classifieds.Common
{
    public class CommonDBRepository : ICommonDBRepository
    {
        private readonly string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["UserProfilesConnectionString"].ConnectionString;
        private readonly string DATABASE = ConfigurationManager.AppSettings["UserProfilesDBName"];
        private readonly string COLLECTION_TOKENS = ConfigurationManager.AppSettings["UserTokensCollection"];

        private MongoClient client = null;
        private MongoServer server = null;
        private MongoDatabase db = null;
        private MongoCollection<UserToken> userTokens = null;

        public CommonDBRepository()
        {
            client = new MongoClient(CONNECTION_STRING);
            server = client.GetServer();
            db = server.GetDatabase(DATABASE);
            userTokens = db.GetCollection<UserToken>(COLLECTION_TOKENS);
        }

        private bool validateRequest(string accessToken, string userEmail)
        {
            try
            {

                var tokens = this.userTokens.FindAll()
                                .Where(p => p.AccessToken == accessToken && p.UserEmail == userEmail)
                                .ToList();
                return tokens.Count == 1 ? true : false;
            }
            catch (Exception ex) { return false; }
        }

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
                    return "200";// new HttpResponseMessage(HttpStatusCode.OK).ToString();
                }
                else
                { return new HttpResponseMessage(HttpStatusCode.Unauthorized).ToString() + " access denied"; }
            }
            catch (Exception ex)
            { return new HttpResponseMessage(HttpStatusCode.Conflict).ToString() + "Pls try some other time."; }
        }

        /// <summary>
        /// Saves a new user token into the database
        /// </summary>
        /// <param name="object">UserToken object</param>
        /// <returns>return token</returns>
        public UserToken SaveToken(UserToken userToken)
        {
            try
            {
                var result = this.userTokens.Save(userToken);
                if (result.DocumentsAffected == 0 && result.HasLastErrorMessage) { }
                return userToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public interface ICommonDBRepository
    {
        string IsAuthenticated(HttpRequestMessage request);
        UserToken SaveToken(UserToken userToken);
    }
}
