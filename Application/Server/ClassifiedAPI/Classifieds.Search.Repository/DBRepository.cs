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

        private MongoClient client = null;
        private MongoServer server = null;
        private MongoDatabase db = null;

        public DBRepository()
        {
            client = new MongoClient(_connectionString);
            server = client.GetServer();
            db = server.GetDatabase(_database);
        }

        protected MongoDatabase Database
        {
            get { return server.GetDatabase(_database); }
        }
    }
}
