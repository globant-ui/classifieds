using Classifieds.UserService.BusinessEntities;
using System.Collections.Generic;

namespace Classifieds.UserService.Repository
{
    public interface ISubscriptionRepository<TEntity> where TEntity : Subscription
    {
        TEntity AddSubscription(TEntity subObject);
        void DeleteSubscription(string id);
    }
}
