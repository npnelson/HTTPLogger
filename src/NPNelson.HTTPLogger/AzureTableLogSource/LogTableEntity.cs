using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NPNelson.HTTPLogger.AzureTableLogSource
{
    public class LogTableEntity:TableEntity
    {
        public LogTableEntity(HttpInfo httpInfo,IEnumerable<LogInfo> messages,string appName,string appVersion)
        {
           
            this.PartitionKey = messages.Min(x => x.Time).ToString("yyyyMMddHH");
            this.RowKey = (httpInfo?.RequestID) ?? Guid.NewGuid().ToString();
            this.Timestamp = messages.Min(x => x.Time);
            this.AppName = appName;
            this.AppVersion = appVersion;
            this.Host = httpInfo?.Host.ToString();
            this.Path = httpInfo?.Path.ToString();
            this.Method = httpInfo?.Method;
            this.Scheme = httpInfo?.Scheme;
            this.StatusCode = httpInfo?.StatusCode??0;
            this.UserName = httpInfo?.User?.Identities.FirstOrDefault(x => !String.IsNullOrWhiteSpace(x.Name))?.Name;
            this.Duration = (messages.Max(x => x.Time) - this.Timestamp).TotalMilliseconds;
            this.RemoteIP = httpInfo?.RemoteIPAddress?.ToString();
            this.HttpInfo = httpInfo?.SerializedString;
            this.Messages = String.Empty;
            foreach (var msg in messages)
            {
                this.Messages += msg.ToString() + "\r\n";
            }

        }

        public string Host { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }
        public string Scheme { get; set; }
        public int StatusCode { get; set; }
        public string UserName { get; set; }
        public double Duration { get; set; }
        public string RemoteIP { get; set; }
        public string AppName { get; set; }
        public string AppVersion { get; set; }

        public string HttpInfo { get; set; }
        public string Messages { get; set; }
    }
}
