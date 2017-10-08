using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers.IHelpers
{
    public interface IApplicationConfig
    {
        string WebServerUrl { get; }

        string BlobConnectionString { get; }
        string CsvContainer { get; }

        string RedisServerName { get; }
        string RedisFileMetaList { get; }
        string RedisConnectionString { get; }

        string AcceptedFiles { get; }

        string RabbitConnection { get; }
        int RabbitConnectionPort { get; }
        string RabbitConnectionUsername { get; }
        string RabbitConnectionPassword { get; }

        string FileDataCreateQueue { get; }
        string FileMetaDeleteQueue { get; }
        string FileOpenedQueue { get; }



    }
}
