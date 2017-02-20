using MongoDB.Driver;

namespace Classifieds.TokenTaskScheduler
{
    public interface ITokenDBRepository
    {
        MongoCollection<UserToken> GetTokenCollection<UserToken>(string name);
    }
}
