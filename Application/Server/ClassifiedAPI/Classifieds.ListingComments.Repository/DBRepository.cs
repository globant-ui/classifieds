# region Imports
using MongoDB.Driver;
using System.Configuration;
#endregion

namespace Classifieds.ListingComments.Repository
{
    public class DBRepository : IDBRepository
    {
        #region Connection String

        private readonly string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["ListingCommentsConnectionString"].ConnectionString;
        private readonly string DATABASE = ConfigurationManager.AppSettings["ListingCommentsDBName"];
        private readonly MongoClient client = null;
        private readonly MongoServer server = null;
        private readonly MongoDatabase db = null;

        #endregion

        #region Constructor
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
        #endregion

        #region GetCollection
        /// <summary>
        /// This methods returns the mongo collection instance representing a collection on database
        /// </summary>
        /// <typeparam name="ListingComments">document type</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public MongoCollection<ListingComment> GetCollection<ListingComment>(string name)
        {
            return db.GetCollection<ListingComment>(name);
        }

        #endregion
    }
}
