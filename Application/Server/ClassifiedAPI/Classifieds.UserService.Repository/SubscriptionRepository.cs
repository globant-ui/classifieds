using Classifieds.UserService.BusinessEntities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;

namespace Classifieds.UserService.Repository
{
    public class SubscriptionRepository<TEntity> : DBRepository, ISubscriptionRepository<TEntity> where TEntity : Subscription
    {
        #region Private Variables
        private readonly string _collectionSubscription = ConfigurationManager.AppSettings["SubscriptionCollection"];
        private readonly IDBRepository _dbRepository;
        MongoCollection<TEntity> Subscription
        {
            get
            {
                return _dbRepository.GetSubscription<TEntity>(_collectionSubscription);
            }
        }
        #endregion

        #region Constructor
        public SubscriptionRepository(IDBRepository dBRepository)
        {
            _dbRepository = dBRepository;
        }
        #endregion

        #region Public Methods

        #region AddSubscription

        /// <summary>
        /// Insert a new Subscription object into the database
        /// </summary>
        /// <param name="subscriptionObj">Subscription object</param>
        /// <returns>Newly added Subscription object</returns>
        public TEntity AddSubscription(TEntity subscriptionObj)
        {
            try
            {
                Subscription.Save(subscriptionObj);
                return subscriptionObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DeleteSubscription
        /// <summary>
        /// Delete Subscription object based on id from the database
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>return void</returns>
        public void DeleteSubscription(string id)
        {
            try
            {
                var query = Query<TEntity>.EQ(p => p._id, id);
                Subscription.Remove(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion DeleteSubscription

        #endregion
    }
}
