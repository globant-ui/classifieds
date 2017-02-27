using System;
using System.Configuration;
using System.Linq;
using Classifieds.Common.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Classifieds.TokenTaskScheduler
{
    public class TokenScheduler
    {
        #region Private variables
        private readonly string _collectionTokens = ConfigurationManager.AppSettings["UserTokensCollection"];
        private readonly ITokenDBRepository _dbRepository;
        private readonly NLog.MongoDB.ILogger _logger;
        #endregion

        MongoCollection<UserToken> UserTokens
        {
            get { return _dbRepository.GetTokenCollection<UserToken>(_collectionTokens); }
        }

        #region Constructor
        public TokenScheduler()
        {
            _dbRepository = new TokenDBRepository();
            _logger = new NLog.MongoDB.Logger();
        }

        #endregion

        #region public methods
        public void PurgeUserTokens()
        {
            try
            {
                var query = Query<UserToken>.Where(p => p.LoginDateTime <= DateTime.Now.AddDays(-1));
                var result = UserTokens.Find(query)
                    .ToList();
                if (result.Count > 0)
                {
                    UserTokens.Remove(query);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Token Scheduler Log");
            }
        }

        #endregion

        public static void Main(string[] args)
        {
            TokenScheduler scheduler = new TokenScheduler();
            scheduler.PurgeUserTokens();
        }
    }
}
