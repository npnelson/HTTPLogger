using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using System.Linq;
using System.Threading.Tasks;

namespace NPNelson.HTTPLogger
{
    public class HTTPLoggerCaptureMiddleware
    {
      
            private readonly RequestDelegate _next;
            private readonly HTTPLoggerOptions _options;
            private readonly ILogger _logger;

            public HTTPLoggerCaptureMiddleware(RequestDelegate next, ILoggerFactory factory, IOptions<HTTPLoggerOptions> options)
            {
                _next = next;
                _options = options.Value;
                _logger = factory.CreateLogger<HTTPLoggerCaptureMiddleware>();
            }

            public async Task Invoke(HttpContext context)
            {
                using (RequestIdentifier.Ensure(context))
                {
                    var requestId = context.Features.Get<IHttpRequestIdentifierFeature>().TraceIdentifier;
                    using (_logger.BeginScope("Request: {RequestId}", requestId))
                    {
                        try
                        {
                            HTTPLoggerScope.Current.Context.HttpInfo = GetHttpInfo(context);
                            await _next(context);
                      
                    }
                        finally
                        {
                        HTTPLoggerScope.Current.Context.HttpInfo.User = context.User; //user won't get set until it is authorized in the pipeline
                        HTTPLoggerScope.Current.Context.HttpInfo.StatusCode = context.Response.StatusCode;
                        }
                    }
                }
            }

            /// <summary>
            /// Takes the info from the given HttpContext and copies it to an HttpInfo object
            /// </summary>
            /// <returns>The HttpInfo for the current httplogger context</returns>
            private static HttpInfo GetHttpInfo(HttpContext context)
            {

           


            return new HttpInfo()
            {
                //User should be done after the fact, probably StatusCode too
                RequestID = context.Features.Get<IHttpRequestIdentifierFeature>().TraceIdentifier,
                Host = context.Request.Host,
                ContentType = context.Request.ContentType,
                Path = context.Request.Path,
                Scheme = context.Request.Scheme,
                StatusCode = context.Response.StatusCode,
                Method = context.Request.Method,
                Protocol = context.Request.Protocol,
                Headers = context.Request.Headers.Where(x=>x.Key!="Cookie").ToList(),
                Query = context.Request.QueryString,               
                Cookies = context.Request.Cookies.Where(x => !x.Key.StartsWith(".AspNet.Cookie")).ToList(),
                RemoteIPAddress = context?.Connection?.RemoteIpAddress?.ToString()   //remoteip won't come across until rc2   
                };
            }
        }
   
}
