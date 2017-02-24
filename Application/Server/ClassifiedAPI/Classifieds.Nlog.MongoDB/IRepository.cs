using NLog;
using System;

namespace Classifieds.NLog.MongoDB
{
	public interface IRepository : IDisposable
	{
		void Insert(LogEventInfo item);
	}
}