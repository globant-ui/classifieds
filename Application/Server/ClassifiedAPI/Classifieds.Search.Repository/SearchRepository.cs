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
    public class SearchRepository : DBRepository, ISearchRepository
    {
        private readonly string _classifiedsCollection = ConfigurationManager.AppSettings["Collection"];
        MongoCollection<Listing> classifieds
        {
            get { return Database.GetCollection<Listing>(_classifiedsCollection); }
        }
    

        public List<Listing> FullTextSearch(string searchText)
        {
            try
            {
                List<Listing> result = new List<Listing>();
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
