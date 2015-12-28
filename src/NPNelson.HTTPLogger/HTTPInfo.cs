using Microsoft.AspNet.Http;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Collections.Generic;

namespace NPNelson.HTTPLogger
{
    public class HttpInfo
    {
        public string RequestID { get; set; }

        public HostString Host { get; set; }

        public PathString Path { get; set; }

        public string ContentType { get; set; }

        public string Scheme { get; set; }

        public int StatusCode { get; set; }

        public ClaimsPrincipal User { get; set; }

        public string Method { get; set; }

        public string Protocol { get; set; }

        public IHeaderDictionary Headers { get; set; }

        public QueryString Query { get; set; }

        public List<KeyValuePair<string,StringValues>> Cookies { get; set; }

        public string RemoteIPAddress { get; set; }
    }
}
