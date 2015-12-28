using System.Collections.Generic;

namespace NPNelson.HTTPLogger.Abstractions
{
    public interface ILogSource
    {
        void WriteLog(HttpInfo httpInfo, IEnumerable<LogInfo> messages);
    }
}
