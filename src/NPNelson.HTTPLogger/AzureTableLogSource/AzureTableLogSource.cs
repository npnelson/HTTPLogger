using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NPNelson.HTTPLogger.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPNelson.HTTPLogger.AzureTableLogSource
{

    public class AzureTableLogSource : ILogSource
    {

        private readonly CloudStorageAccount _storageAccount;
        private readonly string _tableName;
        private readonly string _appName;
        private readonly string _appVersion;
        public AzureTableLogSource(string storageConnectionString,string tableName,string appName,string appVersion)
        {
            _storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            _tableName = tableName;
            _appName = appName;
            _appVersion = appVersion;     
         
        }

        private async Task WriteLogAsync(HttpInfo httpInfo, IEnumerable<LogInfo> messages)
        {

            //should throw some retry logic in here at some point, //should also remove the createtableifnotexists call from here at some point
            var tableClient = _storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(_tableName);
            try
            {
                await table.CreateIfNotExistsAsync();
            }
            catch (Exception ex)
            {
                var temp = ex;
                throw;
            }
            var logEntity = new LogTableEntity(httpInfo, messages, _appName, _appVersion);
            var tableOperation = TableOperation.Insert(logEntity);
            try
            {
                await table.ExecuteAsync(tableOperation);
            }
            catch (Exception ex)
            {
                var temp = ex;
                throw;
            }
        }

        public void WriteLog(HttpInfo httpInfo, IEnumerable<LogInfo> messages)
        {
            WriteLogAsync(httpInfo, messages); //it's okay that it isn't awaited, we don't care
        }
    }
}
