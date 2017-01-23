using System;

namespace Classifieds.NLog.MongoDB
{
    public interface ILogger
    {
        void Log(Exception ex, string userId);
    }
}
