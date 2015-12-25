using Microsoft.Extensions.Logging;
using System;

namespace NPNelson.HTTPLogger
{
    public class HTTPLoggerProvider : ILoggerProvider
    {
     
        private readonly HTTPLoggerOptions _options;

        public HTTPLoggerProvider( HTTPLoggerOptions options)
        {
           

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

          
            _options = options;
        }

        public ILogger CreateLogger(string name)
        {
            return new HTTPLogger(name, _options);
        }

        public void Dispose()
        {
        }
    }
}
