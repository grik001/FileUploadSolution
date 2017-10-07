using Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Service.Hubs
{
    namespace ChatWebApplication.Service.Hubs
    {
        public interface IFileUploadHub
        {
            void VerifyUpload(string targetClient, string id, string name);

            void RemoveFileFromList(string targetClient, string id);

            void UpdateFileRow(string targetClient, FileViewModel file);
        }
    }
}
