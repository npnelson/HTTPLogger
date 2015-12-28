using Microsoft.Extensions.Logging;
using System;

namespace NPNelson.HTTPLogger
{
    public class HTTPLogger : ILogger
    {
        private readonly string _name;
        private readonly HTTPLoggerOptions _options;
        private readonly string _appName;
        private readonly string _appVersion;

        public HTTPLogger(string name, HTTPLoggerOptions options,string appName,string appVersion)
        {
            _name = name;
            _options = options;
            _appName = appName;
            _appVersion = appVersion;

       
        }

        public void Log(LogLevel logLevel, int eventId, object state, Exception exception,
                          Func<object, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel) || (state == null && exception == null))
            {
                return;
            }
            LogInfo info = new LogInfo()
            {
                ActivityContext = GetCurrentActivityContext(),
                Name = _name,
                EventID = eventId,
                Severity = logLevel,
                Exception = exception,
                State = state,
                Message = formatter == null ? state.ToString() : formatter(state, exception),
                Time = DateTimeOffset.UtcNow
            };
            if (HTTPLoggerScope.Current != null)
            {
                HTTPLoggerScope.Current.Node.Messages.Add(info);
            }
            // The log does not belong to any scope - create a new context for it
            else
            {
                var context = GetNewActivityContext();
                context.RepresentsScope = false;  // mark as a non-scope log
                context.Root = new ScopeNode();
                context.Root.Messages.Add(info);
         //       throw new NotImplementedException();
              //  _store.AddActivity(context);
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _options.Filter(_name, logLevel);
        }

        public IDisposable BeginScopeImpl(object state)
        {
            var scope = new HTTPLoggerScope(_name, state);
            scope.Context = HTTPLoggerScope.Current?.Context ?? GetNewActivityContext();
            return HTTPLoggerScope.Push(scope,new AzureTableLogSource.AzureTableLogSource(_options.StorageConnectionString,_options.LogTableName,_appName,_appVersion));
        }

        private ActivityContext GetNewActivityContext()
        {
            return new ActivityContext()
            {
                Id = Guid.NewGuid(),
                Time = DateTimeOffset.UtcNow,
                RepresentsScope = true
            };
        }

        private ActivityContext GetCurrentActivityContext()
        {
            return HTTPLoggerScope.Current?.Context ?? GetNewActivityContext();
        }
    }
}
