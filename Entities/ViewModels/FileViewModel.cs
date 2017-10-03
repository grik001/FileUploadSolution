using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class FileViewModel
    {
        public Guid ID{ get; set; }
        public string UserID { get; set; }
        public string Filename { get; set; }
        public string FileExtension { get; set; }
        public string BlobUrl { get; set; }

        public FileViewModel(Guid id, string userID, string fileName, string fileExtension, string blobUrl)
        {
            this.ID = id;
            this.UserID = userID;
            this.Filename = fileName;
            this.FileExtension = fileExtension;
            this.BlobUrl = blobUrl;
        }
    }
}
