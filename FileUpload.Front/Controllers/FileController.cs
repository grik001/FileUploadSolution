using Data.DataModels;
using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;

namespace FileUpload.Front.Controllers
{
    public class FileController : ApiController
    {
        private IFileDataModel _fileDataModel;

        public FileController()
        {
            this._fileDataModel = new FileDataModel();
        }

        public IHttpActionResult Get()
        {
            try
            {
                var userID = Common.GenericHelpers.GetUserID();
                var result = _fileDataModel.Get(userID);
                return Ok<List<File>>(result);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult Get(Guid id)
        {
            try
            {
                var userID = Common.GenericHelpers.GetUserID();
                var result = _fileDataModel.Get(id);

                if (result != null)
                {
                    return Ok<File>(result);
                }
            }
            catch (Exception ex)
            {
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
                    var result = _fileDataModel.Insert(file);

                    if (result != null)
                    {
                        return Created<File>("", result);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            return BadRequest();
        }

        public IHttpActionResult Put(Guid id, [FromBody]File value)
        {
            try
            {
                var userID = Common.GenericHelpers.GetUserID();

                value.ID = id;
                var result = _fileDataModel.Update(value);

                if (result > 0)
                {
                    return Ok<File>(value);
                }
            }
            catch (Exception ex)
            {
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

                if (result != null)
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            return NotFound();
        }
    }
}