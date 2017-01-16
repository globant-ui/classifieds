using System;
using System.Collections.Generic;
using System.Linq;
using Classifieds.Listings.BusinessEntities;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System.Configuration;

namespace Classifieds.Search.Repository
{
    public class SearchRepository<TEntity> : DBRepository, ISearchRepository<TEntity> where TEntity:Listing
    {
        private readonly string _classifiedsCollection = ConfigurationManager.AppSettings["Collection"];
        MongoCollection<TEntity> classifieds
        {
            get { return Database.GetCollection<TEntity>(typeof(TEntity).Name); }//_classifiedsCollection
        }
    

        public List<TEntity> FullTextSearch(string searchText)
        {
            try
            {
                List<TEntity> result = new List<TEntity>();
                result = this.classifieds.Find(Query.Text(searchText)).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
