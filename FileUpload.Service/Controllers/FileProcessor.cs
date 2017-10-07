using Common.Helpers.IHelpers;
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
        IFileUploadHelper _fileUploadHelper = null;
        IApplicationConfig _applicationConfig = null;

        public FileProcessor(IFileDataModel fileDataModel, IFileUploadHelper fileUploadHelper, IApplicationConfig applicationConfig)
        {
            this._fileDataModel = fileDataModel;
            this._fileUploadHelper = fileUploadHelper;
            this._applicationConfig = applicationConfig;
        }


        public void FilePushed(FileMetaData file)
        {
            _fileDataModel.Insert(file);
        }

        public void FileDelete(FileMetaData file)
        {
            var fileDB = _fileDataModel.Get(file.ID);
            if (fileDB.UserID == file.UserID)
            {
                var deltetedFromBlob = _fileUploadHelper.DeleteFile(_applicationConfig, Convert.ToString(file.ID));
                var fileExists = _fileUploadHelper.Exists(_applicationConfig, Convert.ToString(file.ID));

                if (fileExists == false || deltetedFromBlob)
                {
                    _fileDataModel.Delete(file.ID);
                }
            }
        }

        public void FileOpened(FileMetaData file)
        {
            var fileDB = _fileDataModel.Get(file.ID);
            if (fileDB.UserID == file.UserID)
            {
                fileDB.ViewCount += 1;
                _fileDataModel.Update(fileDB);
            }
        }
    }
}
