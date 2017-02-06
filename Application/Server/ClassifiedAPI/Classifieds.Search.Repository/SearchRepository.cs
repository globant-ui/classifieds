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

        /// <summary>
        /// Performs search operation based on search text
        /// </summary>
        /// <param name="searchText">search query</param>
        /// <param name="startIndex">Start Page no</param>
        /// <param name="pageCount">No of results included</param>
        /// <returns>Collection of listings</returns>
        public List<TEntity> FullTextSearch(string searchText, int startIndex = 1, int pageCount = 10)
        {
            try
            {
                var skip = startIndex - 1;
                List<TEntity> searchResult = Classifieds.Find(Query.Text(searchText))
                                                        .Select(p => p)
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
