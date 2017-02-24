using System;
using System.Configuration;
using System.Linq;
using Classifieds.Listings.BusinessEntities;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Classifieds.ListingTaskScheduler
{
    /// <summary>
    /// Class to carry out maintenance activities related to listings
    /// </summary>
    class ListingScheduler
    {
        #region Private variables
        private readonly string _collectionListings = ConfigurationManager.AppSettings["ListingCollection"];
        private readonly IListingDBRepository _dbRepository;
        private readonly NLog.MongoDB.ILogger _logger;
        private enum Status
        {
            Active,
            Expired
        };
        #endregion

        #region Constructor
        public ListingScheduler()
        {
            _dbRepository = new ListingDBRepository();
            _logger = new NLog.MongoDB.Logger();
        }
        #endregion

        #region public methods
        /// <summary>
        /// Method sets the status of expired listings(more than 1 month old) to Expired
        /// </summary>
        public void DeactivateExpiredListings()
        {
            try
            {
                MongoCollection<Listing> listingCollection =
                    _dbRepository.GetListingCollection<Listing>(_collectionListings);
                var query =
                    Query<Listing>.Where(
                        p => (p.SubmittedDate <= DateTime.Now.AddDays(-30)) && (p.Status == Status.Active.ToString()));
                var result = listingCollection.Find(query)
                                               .ToList();
                if (result.Count > 0)
                {
                    var update = Update.Set("Status", Status.Expired.ToString());
                    var bulkOps = listingCollection.InitializeUnorderedBulkOperation();
                    bulkOps.Find(query).Upsert().Update(update);
                    bulkOps.Execute();
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Listing Scheduler Log");
            }
        }

        #endregion

        public static void Main(string[] args)
        {
            ListingScheduler scheduler = new ListingScheduler();
            scheduler.DeactivateExpiredListings();
        }
    }
}
