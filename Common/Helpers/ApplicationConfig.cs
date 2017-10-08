using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Helpers.IHelpers;
using System.Configuration;

namespace Common.Helpers
{
    public class ApplicationConfig : IApplicationConfig
    {
        public string FileDataCreateQueue { get => ConfigurationManager.AppSettings["FileDataCreateQueue"]; }
        public string FileMetaDeleteQueue { get => ConfigurationManager.AppSettings["FileMetaDeleteQueue"]; }
        public string FileOpenedQueue { get => ConfigurationManager.AppSettings["FileOpenedQueue"]; }

        public string RabbitConnection { get => ConfigurationManager.AppSettings["RabbitConnection"]; }
        public string RedisServerName { get => ConfigurationManager.AppSettings["RedisServerName"]; } 
        public string WebServerUrl { get => ConfigurationManager.AppSettings["WebServerUrl"]; }
        public string BlobConnectionString { get => ConfigurationManager.AppSettings["BlobConnectionString"]; }
        public string CsvContainer { get => ConfigurationManager.AppSettings["CsvContainer"]; }
        public string RedisFileMetaList { get => ConfigurationManager.AppSettings["RedisFileMetaList"]; }
        public string AcceptedFiles { get => ConfigurationManager.AppSettings["AcceptedFiles"]; }
    }
}
