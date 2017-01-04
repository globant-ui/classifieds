using Microsoft.Practices.Unity;
using System.Web.Http;
using Classifieds.Search.BusinessServices;
using Classifieds.Search.Repository;
using Classifieds.Common;
using Mongo = Classifieds.NLog.MongoDB;
using Classifieds.Listings.BusinessServices;
using Classifieds.Listings.Repository;


namespace Classifieds.IOC
{
    /// <summary>
    /// This is the configuration file where you can register interfaces.
    /// we can inject dependancy in constructor.
    /// class name: UnityConfig
    /// Purpose :To implement dependancy injection
    /// Modified by :
    /// Modified date:
    /// </summary>
    public static class UnityConfig
    {
        public static void RegisterComponents(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<ISearchService, SearchService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISearchRepository, SearchRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ILogger, Logger>(new HierarchicalLifetimeManager());
            container.RegisterType<Mongo.ILogger, Mongo.Logger>(new HierarchicalLifetimeManager());

            container.RegisterType<IListingService, ListingService>(new HierarchicalLifetimeManager());
            container.RegisterType<IListingRepository, ListingRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IDBRepository, Listings.Repository.DBRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ILogger, Logger>(new HierarchicalLifetimeManager());
       
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}