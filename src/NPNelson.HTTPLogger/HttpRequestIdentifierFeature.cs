using Microsoft.AspNet.Http.Features;

namespace NPNelson.HTTPLogger
{
    internal class HttpRequestIdentifierFeature : IHttpRequestIdentifierFeature
    {
        public string TraceIdentifier { get; set; }
    }
}
