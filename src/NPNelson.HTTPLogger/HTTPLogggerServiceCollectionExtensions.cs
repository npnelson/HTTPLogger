using NPNelson.HTTPLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HTTPLogggerServiceCollectionExtensions
    {

        public static void ConfigureHTTPLogger(
           this IServiceCollection services,
           Action<HTTPLoggerOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.Configure(configureOptions);
        }

    }
}
