using Classifieds.Common.Entities;
using MongoDB.Driver;

namespace Classifieds.Common.Repositories
{
    public interface ICommonDBRepository
    {
        MongoCollection<UserToken> GetCollection<UserToken>(string name);
    }
}
