using MongoDB.Driver;

namespace Classifieds.ListingTaskScheduler
{
    public interface IListingDBRepository
    {
        MongoCollection<Listing> GetListingCollection<Listing>(string name);
    }
}
