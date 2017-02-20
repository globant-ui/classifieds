using MongoDB.Driver;
using System.Configuration;

namespace Classifieds.TokenTaskScheduler
{
    public class TokenDBRepository : ITokenDBRepository
    {
        #region Private variables
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["UserProfilesConnectionString"].ConnectionString;
        private readonly string _userDatabase = ConfigurationManager.AppSettings["UserProfilesDBName"];

        private readonly MongoClient _client = null;
        private readonly MongoServer _server = null;
        private readonly MongoDatabase _tokenDb = null;
        #endregion

        public TokenDBRepository()
        {
            _client = new MongoClient(_connectionString);
            _server = _client.GetServer();
            _tokenDb = _server.GetDatabase(_userDatabase);
        }

        #region Public Methods
        /// <summary>
        /// This methods returns the mongo collection instance representing a collection on database
        /// </summary>
        /// <typeparam name="UserToken">document type</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public MongoCollection<UserToken> GetTokenCollection<UserToken>(string name)
        {
            return _tokenDb.GetCollection<UserToken>(name);
        }
        #endregion

    }
}
