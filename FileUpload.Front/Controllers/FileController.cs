using Common;
using Common.Helpers.IHelpers;
using Data.DataModels;
using Entities;
using Entities.Models;
using Entities.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace FileUpload.Front.Controllers
{
    [Authorize]
    public class FileController : ApiController
    {
        private IFileDataModel _fileDataModel;
        private IMessageQueueHelper _messageQueueHelper;
        private ILogger _logger;
        private IApplicationConfig _applicationConfig;
        private IFileUploadHelper _fileUploadHelper;
        private IGenericHelper _genericHelper;

        public FileController(IFileDataModel fileDataModel, ILogger logger, IMessageQueueHelper messageQueueHelper, IApplicationConfig applicationConfig, IFileUploadHelper fileUploadHelper, IGenericHelper genericHelper)
        {
            this._fileDataModel = fileDataModel;
            this._logger = logger;
            this._messageQueueHelper = messageQueueHelper;
            this._applicationConfig = applicationConfig;
            this._fileUploadHelper = fileUploadHelper;
            this._genericHelper = genericHelper;
        }

        public IHttpActionResult Get()
        {
            try
            {
                var userID = _genericHelper.GetUserID();

                if (userID != null)
                {
                    var filesDB = _fileDataModel.Get(userID);

                    FileViewModel[] result = null;

                    if (filesDB != null)
                    {
                        result = filesDB.Select(x => new FileViewModel(x.ID, x.UserID, x.Filename, x.FileExtension, x.BlobUrl, x.ViewCount, x.FileSize)).ToArray();
                    }

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("File/Get failed", ex);
                return InternalServerError();
            }

            return NotFound();
        }

        [HttpPost]
        public IHttpActionResult Post()
        {
            try
            {
                List<FileViewModel> pushedfiles = new List<FileViewModel>();
                List<FileViewModel> failedfiles = new List<FileViewModel>();

                var userID = _genericHelper.GetUserID();

                if (userID != null)
                {
                    var files = _genericHelper.GetFilesFromHttpMessage();

                    if (files != null && files.Count > 0)
                    {
                        foreach (var fileContent in files)
                        {
                            var fileID = Guid.NewGuid();

                            if (fileContent != null && fileContent.ContentLength > 0)
                            {
                                var extension = Path.GetExtension(fileContent.FileName);
                                var isFileValid = _genericHelper.IsFileAccepted(_applicationConfig, extension);

                                if (isFileValid == false)
                                {
                                    //do logic to report failed uploads //log for info purposes
                                    continue;
                                }

                                var stream = fileContent.InputStream;
                                var url = _fileUploadHelper.UploadFile(_applicationConfig, stream, fileID.ToString() + extension);

                                if (url != null)
                                {
                                    FileMetaData fileMeta = new FileMetaData();
                                    fileMeta.UserID = userID;
                                    fileMeta.ID = fileID;
                                    fileMeta.BlobUrl = url;
                                    fileMeta.FileSize = fileContent.ContentLength;
                                    fileMeta.Filename = fileContent.FileName;

                                    var socketID = _genericHelper.GetCurrentSocketID();

                                    QueueFileMetaDataModel queueItem = new QueueFileMetaDataModel(fileMeta, socketID);
                                    queueItem.MappingID = fileContent.FileKey;
                                    _messageQueueHelper.PushMessage<QueueFileMetaDataModel>(_applicationConfig, queueItem, _applicationConfig.FileDataCreateQueue);

                                    pushedfiles.Add(new FileViewModel(fileMeta.ID, fileMeta.UserID, fileMeta.Filename, fileMeta.FileExtension, fileMeta.BlobUrl, fileMeta.ViewCount, fileMeta.FileSize));
                                }
                            }
                        }
                    }

                    return Ok(pushedfiles);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"File/Post failed", ex);
                return InternalServerError();
            }

            return BadRequest();
        }

        public IHttpActionResult Put(Guid id)
        {
            try
            {
                var userID = _genericHelper.GetUserID();

                if (userID != null)
                {
                    FileMetaData fileMeta = new FileMetaData();
                    fileMeta.UserID = userID;
                    fileMeta.ID = id;

                    var socketID = _genericHelper.GetCurrentSocketID();
                    QueueFileMetaDataModel queueItem = new QueueFileMetaDataModel(fileMeta, socketID);
                    _messageQueueHelper.PushMessage<QueueFileMetaDataModel>(_applicationConfig, queueItem, _applicationConfig.FileOpenedQueue);

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"File/Put/id  id:{id} failed", ex);
                return InternalServerError();
            }

            return NotFound();
        }

        public IHttpActionResult Delete(Guid id)
        {
            try
            {
                var userID = _genericHelper.GetUserID();

                if (userID != null)
                {
                    FileMetaData fileMeta = new FileMetaData();
                    fileMeta.UserID = userID;
                    fileMeta.ID = id;

                    var socketID = _genericHelper.GetCurrentSocketID();
                    QueueFileMetaDataModel queueItem = new QueueFileMetaDataModel(fileMeta, socketID);

                    _messageQueueHelper.PushMessage<QueueFileMetaDataModel>(_applicationConfig, queueItem, _applicationConfig.FileMetaDeleteQueue);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"File/Delete/id {id.ToString()} failed", ex);
                return InternalServerError();
            }

            return NotFound();
        }
    }
}