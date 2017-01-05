using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifieds.Listings.Repository
{
    public class DBRepository
    {

        private string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["ListingConnectionString"].ConnectionString;
        private string DATABASE = ConfigurationManager.AppSettings["ListingDBName"];

        private MongoClient client = null;
        private MongoServer server = null;
        private MongoDatabase db = null;

        public DBRepository()
        {
            client = new MongoClient(CONNECTION_STRING);
            server = client.GetServer();
            db = server.GetDatabase(DATABASE);
        }

        protected MongoDatabase Database
        {
            get { return server.GetDatabase(DATABASE); }
        }
    }
}
