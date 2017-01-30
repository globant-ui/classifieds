using MongoDB.Driver;
using System.Configuration;

namespace Classifieds.Common.Repositories
{
    public class CommonDBRepository : ICommonDBRepository
    {
        #region Private variables
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["UserProfilesConnectionString"].ConnectionString;
        private readonly string _database = ConfigurationManager.AppSettings["UserProfilesDBName"];

        private MongoClient _client = null;
        private MongoServer _server = null;
        private MongoDatabase _db = null;
        #endregion

        #region Constructor
        public CommonDBRepository()
        {
            _client = new MongoClient(_connectionString);
            _server = _client.GetServer();
            _db = _server.GetDatabase(_database);
        }
        #endregion

        /// <summary>
        /// gets a mongodatabase instance representing a database on the server
        /// </summary>
        protected MongoDatabase Database
        {
            get
            {
                return _server.GetDatabase(_database);
            }
        }

        #region Public Methods
        /// <summary>
        /// This methods returns the mongo collection instance representing a collection on database
        /// </summary>
        /// <typeparam name="UserToken">document type</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public MongoCollection<UserToken> GetCollection<UserToken>(string name)
        {
            return _db.GetCollection<UserToken>(name);
        }
        #endregion

    }
}
