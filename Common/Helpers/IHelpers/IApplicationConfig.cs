using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers.IHelpers
{
    public interface IApplicationConfig
    {
        string RabbitConnection { get; }

        string FileDataCreateQueue { get; }
        string FileMetaDeleteQueue { get; }
        string FileOpenedQueue { get; }
        
        string RedisServerName { get; }
        string WebServerUrl { get; }
        string BlobConnectionString { get; }
        string CsvContainer { get; }

        string RedisFileMetaList { get; }
    }
}
