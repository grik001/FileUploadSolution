using Entities.ViewModels;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Service.Hubs
{
    namespace ChatWebApplication.Service.Hubs
    {
        public class FileUploadHub : Hub, IFileUploadHub
        {
            IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<FileUploadHub>();

            public FileUploadHub()
            {
            }

            public void VerifyUpload(string targetClient, string id)
            {
                hubContext.Clients.Client(targetClient).fileuploadUpdateProgress(id);
            }

            public void RemoveFileFromList(string targetClient, string id)
            {
                hubContext.Clients.Client(targetClient).fileuploadRemoveFileFrontList(id);
            }

            public void UpdateFileRow(string targetClient, FileViewModel file)
            {
                hubContext.Clients.Client(targetClient).fileuploadUpdateRow(file);
            }

            #region Overrides
            public override Task OnConnected()
            {
                return (base.OnConnected());
            }

            public override Task OnDisconnected(bool val)
            {
                return (base.OnDisconnected(val));
            }

            public override Task OnReconnected()
            {
                return (base.OnDisconnected(false));
            }
            #endregion
        }
    }
}
