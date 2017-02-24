# region Imports
using MongoDB.Driver;
using System.Configuration;


#endregion

namespace Classifieds.MastersData.Repository
{
    public class DBRepository : IDBRepository
    {
        #region Connection String

        private readonly string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["MasterDataConnectionString"].ConnectionString;
        private readonly string DATABASE = ConfigurationManager.AppSettings["MasterDataDBName"];
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
        #endregion

        protected MongoDatabase Database
        {
            get { return server.GetDatabase(DATABASE); }
        }

        #region GetCollection
        /// <summary>
        /// This methods returns the mongo collection instance representing a collection on database
        /// </summary>
        /// <typeparam name="Categorys">document type</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public MongoCollection<Category> GetCollection<Category>(string name)
        {
            return db.GetCollection<Category>(name);
        }

        #endregion


    }
}
