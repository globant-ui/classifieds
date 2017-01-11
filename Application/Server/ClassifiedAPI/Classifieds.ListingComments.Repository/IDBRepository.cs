using MongoDB.Driver;

namespace Classifieds.ListingComments.Repository
{
    public interface IDBRepository
    {
        MongoCollection<ListingComment> GetCollection<ListingComment>(string name);
    }
}
