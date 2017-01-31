using MongoDB.Driver;
using System.Configuration;

namespace Classifieds.ListingComments.Repository
{
    public class DBRepository : IDBRepository
    {
        #region Connection String

        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["ListingCommentsConnectionString"].ConnectionString;
        private readonly string _database = ConfigurationManager.AppSettings["ListingCommentsDBName"];
        private readonly MongoClient _client = null;
        private readonly MongoServer _server = null;
        private readonly MongoDatabase _db = null;

        #endregion

        #region Constructor
        public DBRepository()
        {
            _client = new MongoClient(_connectionString);
            _server = _client.GetServer();
            _db = _server.GetDatabase(_database);
        }  
        
        protected MongoDatabase Database
        {
            get { return _server.GetDatabase(_database); }
        }
        #endregion

        #region GetCollection
        /// <summary>
        /// This methods returns the mongo collection instance representing a collection on database
        /// </summary>
        /// <typeparam name="ListingComment">document type</typeparam>
        /// <param name="name"></param>
        /// <returns>Name of Collection</returns>
        public MongoCollection<ListingComment> GetCollection<ListingComment>(string name)
        {
            return _db.GetCollection<ListingComment>(name);
        }

        #endregion GetCollection
    }
}
