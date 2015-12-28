using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace NPNelson.HTTPLogger
{
    public class HTTPLoggerProvider : ILoggerProvider
    {
     
        private readonly HTTPLoggerOptions _options;
        private readonly string _appName;
        private readonly string _appVersion;

        public HTTPLoggerProvider( HTTPLoggerOptions options,string appName,string appVersion)
        {
           

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

          
            _options = options;
            _appName = appName;
            _appVersion = appVersion;
        }

     



        public ILogger CreateLogger(string name)
        {
            return new HTTPLogger(name, _options,_appName,_appVersion);
        }

        public void Dispose()
        {
        }
    }
}
