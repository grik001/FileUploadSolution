using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Helpers.IHelpers;

namespace Common.Helpers
{
    public class ApplicationConfig : IApplicationConfig
    {
        public string FileMetaDataQueue { get => "fileMetaDataQueue"; }
        public string FileMetaDeleteQueue { get => "fileMetaDeleteQueue"; }
        public string RabbitConnection { get => "localhost"; }
        public string RedisServerName { get => "localhost:6379"; } //192.168.99.100:32770
        public string WebServerUrl { get => "http://localhost:8090"; }
        public string BlobConnectionString { get => "DefaultEndpointsProtocol=https;AccountName=fileupload001;AccountKey=XwehxPZtZUQqSftaA7werJlKHeEm+bcbvGAE/k7aYJv3o73DsDYt/V73eNs66hMS5pWyRkDVXQqzFgpdhHTnEg==;EndpointSuffix=core.windows.net"; }
        public string CsvContainer { get => "csvcontainer"; }
        
    }
}
