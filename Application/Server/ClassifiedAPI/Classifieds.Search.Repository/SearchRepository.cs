using System;
using System.Collections.Generic;
using System.Linq;
using Classifieds.Listings.BusinessEntities;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Classifieds.Search.Repository
{
    public class SearchRepository<TEntity> : DBRepository, ISearchRepository<TEntity> where TEntity:Listing
    {
        MongoCollection<TEntity> Classifieds
        {
            get
            {
                return Database.GetCollection<TEntity>(typeof(TEntity).Name);
            }
        }
    

        public List<TEntity> FullTextSearch(string searchText, int startIndex, int pageCount)
        {
            try
            {
                var skip = startIndex - 1;
                List<TEntity> result = Classifieds.Find(Query.Text(searchText)).ToList();
                List<TEntity> searchResult = result.Select(p => p)
                                                 .Skip(skip)
                                                 .Take(pageCount)
                                                 .ToList();
                return searchResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
