using System;
using System.Collections.Generic;
using System.Linq;
using Classifieds.Search.BusinessEntities;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Classifieds.Search.Repository
{
    public class SearchRepository : ISearchRepository
    {
        private const string CONNECTION_STRING = "mongodb://localhost";

        private const string DATABASE = "classifiedDB";
        private const string COLLECTION_Classifieds = "classified";

        private MongoClient client = null;
        private MongoServer server = null;
        private MongoDatabase db = null;
        private MongoCollection<Classified> classifieds = null;

        public SearchRepository()
        {
            client = new MongoClient(CONNECTION_STRING);
            server = client.GetServer();
            db = server.GetDatabase(DATABASE);
            classifieds = db.GetCollection<Classified>(COLLECTION_Classifieds);
        }

        public List<Classified> FullTextSearch(string searchText)
        {
            try
            {
                List<Classified> result = new List<Classified>();
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
