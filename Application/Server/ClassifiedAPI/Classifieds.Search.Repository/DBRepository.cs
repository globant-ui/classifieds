using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Configuration;

namespace Classifieds.Search.Repository
{
    public class DBRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["SearchDBConnectionString"].ConnectionString;
        private readonly string _database = ConfigurationManager.AppSettings["SearchDB"];

        private readonly MongoClient _client = null;
        private readonly MongoServer _server = null;
        private readonly MongoDatabase _db = null;

        public DBRepository()
        {
            _client = new MongoClient(_connectionString);
            _server = _client.GetServer();
            _db = _server.GetDatabase(_database);
        }

        protected MongoDatabase Database
        {
            get { return _server.GetDatabase(_database); }
        }
    }
}
