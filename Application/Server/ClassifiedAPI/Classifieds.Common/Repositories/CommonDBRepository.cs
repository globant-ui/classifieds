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

        private MongoClient client = null;
        private MongoServer server = null;
        private MongoDatabase db = null;
        #endregion

        #region Constructor
        public CommonDBRepository()
        {
            client = new MongoClient(_connectionString);
            server = client.GetServer();
            db = server.GetDatabase(_database);
        }
        #endregion

        /// <summary>
        /// gets a mongodatabase instance representing a database on the server
        /// </summary>
        protected MongoDatabase Database
        {
            get
            {
                return server.GetDatabase(_database);
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
            return db.GetCollection<UserToken>(name);
        }
        #endregion

    }
}
