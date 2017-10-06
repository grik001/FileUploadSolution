using Data.DataModels;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Service.Controllers
{
    public class FileProcessor
    {
        IFileDataModel _fileDataModel = null;

        public FileProcessor(IFileDataModel fileDataModel)
        {
            this._fileDataModel = fileDataModel;
        }


        public void FilePushed(FileMetaData value)
        {
            _fileDataModel.Insert(value);
        }

        public void FileDelete(Guid value)
        {
            _fileDataModel.Delete(value);
        }
    }
}
