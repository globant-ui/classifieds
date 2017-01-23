using System;
using MongoDB.Driver;
using NLog;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace Classifieds.NLog.MongoDB
{
    public class Logger : ILogger
    {
       

        NLog.MongoDB.MongoRepository objnlogmongo =
               new NLog.MongoDB.MongoRepository(MongoServerSettings.FromUrl(
                   new MongoUrl(ConfigurationManager.ConnectionStrings["LoggingDBConnectionString"].ConnectionString)), ConfigurationManager.AppSettings["Logging"]);
        void ILogger.Log(Exception ex,string userId)
        {
            string LoggerName = ConfigurationManager.AppSettings["Logging"].ToString();
            LogEventInfo eventdata = new LogEventInfo(LogLevel.Error, LoggerName, null, ex.Message, null, ex);
            eventdata.TimeStamp = DateTime.Now;
            eventdata.Properties["stacktrace"] = ex.StackTrace.ToString();
            eventdata.Properties["UserId"] = userId;
            objnlogmongo.Insert(eventdata);
        }
    }
}
