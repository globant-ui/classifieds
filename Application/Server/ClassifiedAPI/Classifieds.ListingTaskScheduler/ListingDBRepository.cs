using MongoDB.Driver;
using System.Configuration;

namespace Classifieds.ListingTaskScheduler
{
    public class ListingDBRepository : IListingDBRepository
    {
        #region Private variables
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["ListingsConnectionString"].ConnectionString;
        private readonly string _listingDatabase = ConfigurationManager.AppSettings["ListingDBName"];

        private readonly MongoClient _client = null;
        private readonly MongoServer _server = null;
        private readonly MongoDatabase _listingDb = null;
        #endregion

        public ListingDBRepository()
        {
            _client = new MongoClient(_connectionString);
            _server = _client.GetServer();
            _listingDb = _server.GetDatabase(_listingDatabase);
        }

        #region Public Methods

        /// <summary>
        /// This methods returns the mongo collection instance representing a collection on database
        /// </summary>
        /// <typeparam name="Listing">document type</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public MongoCollection<Listing> GetListingCollection<Listing>(string name)
        {
            return _listingDb.GetCollection<Listing>(name);
        }
        #endregion

    }
}
