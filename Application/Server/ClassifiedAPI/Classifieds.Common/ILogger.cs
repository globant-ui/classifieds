using System;

namespace Classifieds.Common
{
    public interface ILogger
    {
        Exception Log(Exception ex, string userId);
    }
}
