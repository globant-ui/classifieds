using MongoDB.Driver;

namespace Classifieds.NLog.MongoDB
{
	public interface IRepositoryProvider
	{
		IRepository GetRepository(MongoServerSettings setting,
		                          string database);
	}
}