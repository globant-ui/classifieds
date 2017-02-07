using Microsoft.Practices.Unity;
using System.Web.Http;
using Classifieds.Search.BusinessServices;
using Classifieds.Search.Repository;
using Classifieds.Common.Repositories;
using Classifieds.Common;
using Mongo = Classifieds.NLog.MongoDB;
using Classifieds.Listings.BusinessServices;
using Classifieds.Listings.Repository;
using Classifieds.MastersData.BusinessServices;
using Classifieds.MastersData.Repository;
using Classifieds.Listings.BusinessEntities;
using Classifieds.MastersData.BusinessEntities;
using Classifieds.UserService.BusinessServices;
using Classifieds.UserService.Repository;
using Classifieds.ListingComments.BusinessServices;
using Classifieds.ListingComments.BusinessEntities;
using Classifieds.ListingComments.Repository;

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
            container.RegisterType<ISearchRepository<Listing>, SearchRepository<Listing>>(new HierarchicalLifetimeManager());
            container.RegisterType<ILogger, Logger>(new HierarchicalLifetimeManager());
            container.RegisterType<Mongo.ILogger, Mongo.Logger>(new HierarchicalLifetimeManager());

            container.RegisterType<IListingService, ListingService>(new HierarchicalLifetimeManager());
            container.RegisterType<IListingRepository<Listing>, ListingRepository<Listing>>(new HierarchicalLifetimeManager());
            container.RegisterType<Listings.Repository.IDBRepository, Listings.Repository.DBRepository>(new HierarchicalLifetimeManager());

            container.RegisterType<IMasterDataService, MasterDataService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMasterDataRepository<Category>, MasterDataRepository<Category>>(new HierarchicalLifetimeManager());
            container.RegisterType<MastersData.Repository.IDBRepository, MastersData.Repository.DBRepository>(new HierarchicalLifetimeManager());

            container.RegisterType<IUserService, Classifieds.UserService.BusinessServices.UserService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<UserService.Repository.IDBRepository, UserService.Repository.DBRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICommonRepository, CommonRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICommonDBRepository, CommonDBRepository>(new HierarchicalLifetimeManager());

            container.RegisterType<IListingCommentService, ListingCommentService>(new HierarchicalLifetimeManager());
            container.RegisterType<IListingCommentsRepository<ListingComment>, ListingCommentsRepository<ListingComment>>(new HierarchicalLifetimeManager());
            container.RegisterType<ListingComments.Repository.IDBRepository, ListingComments.Repository.DBRepository>(new HierarchicalLifetimeManager());

            config.DependencyResolver = new UnityResolver(container);
        }
    }
}