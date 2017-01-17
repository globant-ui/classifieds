using System;

namespace Classifieds.Common
{
    public class Logger: ILogger
    {
        #region Private Variable
        private Classifieds.NLog.MongoDB.ILogger _logger;
        #endregion

        #region Constructor
        public Logger(Classifieds.NLog.MongoDB.ILogger logger)
        {
            _logger = logger;
            //_logger = new Classifieds.NLog.MongoDB.Logger();
        }
        #endregion

        #region Public Method
        /// <summary>
        /// this method used to log exception occures runtime
        /// </summary>
        /// <param name="ex">Exception object</param>
        /// <param name="userId">Logged in user's Id</param>
        /// <returns></returns>
        public Exception Log(Exception ex, string userId)
        {
            _logger.Log(ex, userId);
            return ex;
        }
        #endregion
    }
}
