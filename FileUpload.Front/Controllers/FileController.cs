using Common.Helpers.IHelpers;
using Data.DataModels;
using Entities;
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
    public class FileController : ApiController
    {
        private IFileDataModel _fileDataModel;
        private IMessageQueueHelper _messageQueueHelper;
        private ILogger _logger;
        private IApplicationConfig _applicationConfig;
        private IFileUploadHelper _fileUploadHelper;

        public FileController(IFileDataModel fileDataModel, ILogger logger, IMessageQueueHelper messageQueueHelper, IApplicationConfig applicationConfig, IFileUploadHelper fileUploadHelper)
        {
            this._fileDataModel = fileDataModel;
            this._logger = logger;
            this._messageQueueHelper = messageQueueHelper;
            this._applicationConfig = applicationConfig;
            this._fileUploadHelper = fileUploadHelper;
        }

        public IHttpActionResult Get()
        {
            try
            {
                var userID = Common.GenericHelpers.GetUserID();
                var filesDB = _fileDataModel.Get(userID);

                FileViewModel[] result = null;

                if (filesDB != null)
                {
                    result = filesDB.Select(x => new FileViewModel(x.ID, x.UserID, x.Filename, x.FileExtension, x.BlobUrl, x.ViewCount, x.FileSize)).ToArray();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("File/Get failed", ex);
                return InternalServerError();
            }
        }

        public IHttpActionResult Get(Guid id)
        {
            try
            {
                var userID = Common.GenericHelpers.GetUserID();
                var fileDB = _fileDataModel.Get(id);

                if (fileDB != null)
                {
                    var result = new FileViewModel(fileDB.ID, fileDB.UserID, fileDB.Filename, fileDB.FileExtension, fileDB.BlobUrl, fileDB.ViewCount, fileDB.FileSize);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"File/Get/id {id.ToString()} failed", ex);
                return InternalServerError();
            }

            return NotFound();
        }

        [HttpPost]
        public IHttpActionResult Post()
        {
            try
            {
                var userID = Common.GenericHelpers.GetUserID();

                if (userID != null)
                {
                    var files = HttpContext.Current.Request.Files;

                    if (files.Count > 0)
                    {
                        foreach (string file in files)
                        {
                            var fileID = Guid.NewGuid();

                            var fileContent = files[file];
                            if (fileContent != null && fileContent.ContentLength > 0)
                            {
                                var stream = fileContent.InputStream;

                               var url = _fileUploadHelper.UploadFile(_applicationConfig, stream, fileID.ToString());

                                FileMetaData fileMeta = new FileMetaData();
                                fileMeta.UserID = userID;
                                fileMeta.ID = fileID;
                                fileMeta.BlobUrl = url;

                                _messageQueueHelper.PushMessage<FileMetaData>(_applicationConfig, fileMeta, _applicationConfig.FileMetaDataQueue);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"File/Post failed", ex);
                return InternalServerError();
            }

            return BadRequest();
        }

        public IHttpActionResult Put(Guid id, [FromBody]FileMetaData file)
        {
            try
            {
                var userID = Common.GenericHelpers.GetUserID();
                file.ID = id;

                var result = _fileDataModel.Update(file);

                if (result > 0)
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"File/Put/id  id:{id} - file:{JsonConvert.SerializeObject(file)} failed", ex);
                return InternalServerError();
            }

            return NotFound();
        }

        public IHttpActionResult Delete(Guid id)
        {
            try
            {
                var userID = Common.GenericHelpers.GetUserID();

                if (userID != null)
                {
                    _messageQueueHelper.PushMessage<Guid>(_applicationConfig, id, _applicationConfig.FileMetaDeleteQueue);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"File/Delete/id {id.ToString()} failed", ex);
                return InternalServerError();
            }
        }
    }
}