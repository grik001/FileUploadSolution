using Common.Helpers.IHelpers;
using Data.DataModels;
using Entities;
using Entities.Models;
using Entities.ViewModels;
using FileUpload.Service.Hubs.ChatWebApplication.Service.Hubs;
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
        IFileUploadHub _fileUploadHub = null;

        public FileProcessor(IFileDataModel fileDataModel, IFileUploadHelper fileUploadHelper, IApplicationConfig applicationConfig, IFileUploadHub fileUploadHub)
        {
            this._fileDataModel = fileDataModel;
            this._fileUploadHelper = fileUploadHelper;
            this._applicationConfig = applicationConfig;
            this._fileUploadHub = fileUploadHub;
        }


        public void FilePushed(QueueFileMetaDataModel queueMessage)
        {
            _fileUploadHub.VerifyUpload(queueMessage.SocketID, queueMessage.MappingID);
            _fileDataModel.Insert(queueMessage.FileMeta);
        }

        public void FileDelete(QueueFileMetaDataModel queueMessage)
        {
            var fileDB = _fileDataModel.Get(queueMessage.FileMeta.ID);
            if (fileDB.UserID == queueMessage.FileMeta.UserID)
            {
                var deltetedFromBlob = _fileUploadHelper.DeleteFile(_applicationConfig, Convert.ToString(queueMessage.FileMeta.ID));
                var fileExists = _fileUploadHelper.Exists(_applicationConfig, Convert.ToString(queueMessage.FileMeta.ID));

                if (fileExists == false || deltetedFromBlob)
                {
                    _fileDataModel.Delete(queueMessage.FileMeta.ID);
                    _fileUploadHub.RemoveFileFromList(queueMessage.SocketID, Convert.ToString(queueMessage.FileMeta.ID));
                }
            }
        }

        public void FileOpened(QueueFileMetaDataModel queueMessage)
        {
            var fileDB = _fileDataModel.Get(queueMessage.FileMeta.ID);
            if (fileDB.UserID == queueMessage.FileMeta.UserID)
            {
                fileDB.ViewCount += 1;
                _fileDataModel.Update(fileDB);
                var fileMeta = queueMessage.FileMeta;
                _fileUploadHub.UpdateFileRow(queueMessage.SocketID, new FileViewModel(fileDB.ID, fileDB.UserID, fileDB.Filename, fileDB.FileExtension, fileDB.BlobUrl, fileDB.ViewCount, fileDB.FileSize));
            }
        }
    }
}
