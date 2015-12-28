using Microsoft.AspNet.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using System;

namespace NPNelson.HTTPLogger
{
    public static class HTTPLoggerExtensions
    {
       
        public static IApplicationBuilder UseHTTPLogger(this IApplicationBuilder builder,string appName,string appVersion)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
           
            var factory = builder.ApplicationServices.GetRequiredService<ILoggerFactory>();         
            var options = builder.ApplicationServices.GetService<IOptions<HTTPLoggerOptions>>();
            if (options.Value == null) throw new InvalidOperationException("HTTPLoggerOptions must be configured");
            factory.AddProvider(new HTTPLoggerProvider( options.Value,appName,appVersion));

            return builder.UseMiddleware<HTTPLoggerCaptureMiddleware>();
        }

   
    }
}
