using System;

namespace Classifieds.Common
{
    public interface ILogger
    {
        void Log(Exception ex, string userId);
    }
}
