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
        public Func<string, LogLevel, bool> Filter { get; set; } = (name, level) => true;
    }
}
