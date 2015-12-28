using Microsoft.AspNet.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using System;

namespace NPNelson.HTTPLogger
{
    public static class HTTPLoggerExtensions
    {
       
        public static IApplicationBuilder UseHTTPLogger(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            // add the elm provider to the factory here so the logger can start capturing logs immediately
            var factory = builder.ApplicationServices.GetRequiredService<ILoggerFactory>();         
            var options = builder.ApplicationServices.GetService<IOptions<HTTPLoggerOptions>>();
            factory.AddProvider(new HTTPLoggerProvider( options?.Value ?? new HTTPLoggerOptions()));

            return builder.UseMiddleware<HTTPLoggerCaptureMiddleware>();
        }

   
    }
}
