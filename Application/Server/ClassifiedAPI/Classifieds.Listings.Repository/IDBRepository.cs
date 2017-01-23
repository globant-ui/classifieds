using MongoDB.Driver;

namespace Classifieds.Listings.Repository
{
    public interface IDBRepository
    {
        MongoCollection<Listing> GetCollection<Listing>(string name);
    }
}
