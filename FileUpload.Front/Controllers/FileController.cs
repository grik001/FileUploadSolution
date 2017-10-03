using Common.Helpers.IHelpers;
using Data.DataModels;
using Entities;
using Entities.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace FileUpload.Front.Controllers
{
    public class FileController : ApiController
    {
        private IFileDataModel _fileDataModel;
        private ILogger _logger;

        public FileController(IFileDataModel fileDataModel, ILogger logger)
        {
            this._fileDataModel = fileDataModel;
            this._logger = logger;
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
                    result = filesDB.Select(x => new FileViewModel(x.ID, x.UserID, x.Filename, x.FileExtension, x.BlobUrl)).ToArray();
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
                    var result = new FileViewModel(fileDB.ID, fileDB.UserID, fileDB.Filename, fileDB.FileExtension, fileDB.BlobUrl);
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

        public IHttpActionResult Post([FromBody]File file)
        {
            try
            {
                var userID = Common.GenericHelpers.GetUserID();

                if (userID != null)
                {
                    file.UserID = userID;
                    file.ID = Guid.NewGuid();

                    var fileDB = _fileDataModel.Insert(file);

                    if (fileDB != null)
                    {
                        var result = new FileViewModel(fileDB.ID, fileDB.UserID, fileDB.Filename, fileDB.FileExtension, fileDB.BlobUrl);
                        return Created("", result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"File/Post {JsonConvert.SerializeObject(file)} failed", ex);
                return InternalServerError();
            }

            return BadRequest();
        }

        public IHttpActionResult Put(Guid id, [FromBody]File file)
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
                var result = _fileDataModel.Delete(id);

                if (result == true)
                {
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