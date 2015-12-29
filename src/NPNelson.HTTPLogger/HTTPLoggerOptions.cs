using Microsoft.Extensions.Logging;
using System;

namespace NPNelson.HTTPLogger
{
    public class HTTPLoggerOptions
    {      
        /// <summary>
        /// Determines whether log statements should be logged based on the name of the logger
        /// and the <see cref="LogLevel"/> of the message.
        /// </summary>
        public Func<string, LogLevel, bool> Filter
        {

            get
            {
                return (name, level) => level >= LogLevel;
            }
        }

        public string StorageConnectionString { get; set; }

        public string LogTableName { get; set; }

        public LogLevel LogLevel { get; set; }
    }
}
