﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FileUpload.Front.Controllers
{
    [Authorize]
    public class FileController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return null;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return null;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}