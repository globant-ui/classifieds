using MongoDB.Driver;
using System.Configuration;

namespace Classifieds.UserService.Repository
{
    public class DBRepository : IDBRepository
    {
        #region Private Variables
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["UserProfilesConnectionString"].ConnectionString;
        private readonly string _database = ConfigurationManager.AppSettings["UserProfilesDBName"];

        private readonly MongoClient client = null;
        private readonly MongoServer server = null;
        private readonly MongoDatabase db = null;
        #endregion

        #region Constructor
        public DBRepository()
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
            get { return server.GetDatabase(_database); }
        }

        #region Public Methods
        /// <summary>
        /// This methods returns the mongo collection instance representing a collection on database
        /// </summary>
        /// <typeparam name="Listing">document type</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public MongoCollection<ClassifiedsUser> GetCollection<ClassifiedsUser>(string name)
        {
            return db.GetCollection<ClassifiedsUser>(name);
        }
        #endregion
    }
}
