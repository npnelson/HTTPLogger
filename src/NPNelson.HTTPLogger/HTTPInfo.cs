using Microsoft.AspNet.Http;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

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

        public List<KeyValuePair<string,StringValues>> Headers { get; set; }

        public QueryString Query { get; set; }

        public List<KeyValuePair<string, StringValues>> Cookies { get; set; }

        public string RemoteIPAddress { get; set; }

        public string SerializedString { get { return JsonConvert.SerializeObject(new HttpInfoToSerialize(this), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore}); } }

        private class HttpInfoToSerialize
        {
            public HttpInfoToSerialize(HttpInfo httpInfo)
            {
                this.RequestID = httpInfo.RequestID;
                this.Host = httpInfo.Host;
                this.Path = httpInfo.Path;
                this.ContentType = httpInfo.ContentType;
                this.Scheme = httpInfo.Scheme;
               // this.Claims=httpInfo.User.Claims.Where(x => x.Type != "access_token" && x.Type != "id_token" && x.Type != "nonce" && x.Type!="Bearer" && x.Issuer!="LOCAL AUTHORITY" && x.Type!= "azure_id" && x.Type != "_id").Take(3).ToList(); //dont serialize some of these, they can be too long for one
                this.StatusCode = httpInfo.StatusCode;
                this.Method = httpInfo.Method;
                this.Protocol = httpInfo.Protocol;
                this.Headers = httpInfo.Headers;
                this.Query = httpInfo.Query;
                this.Cookies = httpInfo.Cookies;
                this.RemoteIPAddress = httpInfo.RemoteIPAddress;
            }

            public string RequestID { get; set; }

            public HostString Host { get; set; }

            public PathString Path { get; set; }

            public string ContentType { get; set; }

            public string Scheme { get; set; }

            public int StatusCode { get; set; }

            public List<Claim> Claims { get; set; }

            public string Method { get; set; }

            public string Protocol { get; set; }

            public List<KeyValuePair<string, StringValues>> Headers { get; set; }

            public QueryString Query { get; set; }

            public List<KeyValuePair<string, StringValues>> Cookies { get; set; }

            public string RemoteIPAddress { get; set; }
        }
    }
}
