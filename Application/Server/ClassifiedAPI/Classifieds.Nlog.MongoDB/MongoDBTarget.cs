using System;
using MongoDB.Driver;
using NLog;
using NLog.Targets;
using System.Threading;
using NLog.Common;

namespace Classifieds.NLog.MongoDB
{
    [Target("MongoDB")]
    public sealed class MongoDBTarget : Target
    {
        public Func<IRepositoryProvider> Provider = () => new MongoServerProvider();

        public string Host
        {
            get { return _Host ?? "localhost"; }
            set { _Host = value; }
        }
        private string _Host;

        public int Port
        {
            get { return _Port ?? 27017; }
            set { _Port = value; }
        }
        private int? _Port;

        public string Database
        {
            get { return _Database ?? "Logging"; }
            set { _Database = value; }
        }
        private string _Database;
       
        public void TestWrite(LogEventInfo logEvent)
        {
            Write(logEvent);
        }

        protected override void Write(LogEventInfo logEvent)
        {
            try
            {
                using (var repository = Provider().GetRepository(
                            new MongoServerSettings
                            {
                                Server = new MongoServerAddress(this.Host, this.Port)
                            },
                            this.Database))
                {
                    repository.Insert(logEvent);
                }
            }
            catch (Exception ex)
            {
                if (ex is StackOverflowException || ex is ThreadAbortException || ex is OutOfMemoryException || ex is NLogConfigurationException)
                    throw;

                InternalLogger.Error("Error when writing to MongoDB {0}", ex);
            }
        }
    }
}
