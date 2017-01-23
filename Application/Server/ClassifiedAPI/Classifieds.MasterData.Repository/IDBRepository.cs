using MongoDB.Driver;

namespace Classifieds.MastersData.Repository
{
    public interface IDBRepository
    {
        MongoCollection<Category> GetCollection<Category>(string name);
    }
}
