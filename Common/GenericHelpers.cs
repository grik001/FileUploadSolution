using Common.Helpers;
using Common.Helpers.IHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public interface IGenericHelper
    {
        string GetUserID();
        string GetCurrentSocketID();
        List<HttpFileUploadHelper> GetFilesFromHttpMessage();
        bool IsFileAccepted(IApplicationConfig config, string extension);
    }

    public class GenericHelpers : IGenericHelper
    {
        public bool IsFileAccepted(IApplicationConfig config, string extension)
        {
            var acceptedFiles = config.AcceptedFiles.Split(',');
            return acceptedFiles.Any(x => x.ToLower() == extension.ToLower());
        }

        public List<HttpFileUploadHelper> GetFilesFromHttpMessage()
        {
            List<HttpFileUploadHelper> httpFiles = new List<HttpFileUploadHelper>();

            if (HttpContext.Current != null)
            {
                foreach (string fileKey in HttpContext.Current.Request.Files)
                {
                    var file = HttpContext.Current.Request.Files[fileKey];
                    HttpFileUploadHelper httpFile = new HttpFileUploadHelper(file, fileKey);
                    httpFiles.Add(httpFile);
                }

                return httpFiles;
            }

            return null;
        }

        public string GetUserID()
        {
            if (HttpContext.Current != null)
            {
                var userID = HttpContext.Current.Request.Cookies["UserID"].Value;

                if (userID != null)
                {
                    return Convert.ToString(userID);
                }
            }

            return null;
        }

        public string GetCurrentSocketID()
        {
            if (HttpContext.Current != null)
            {
                var connectionID = HttpContext.Current.Request.Cookies["ConnectionID"].Value;

                if (connectionID != null)
                {
                    return Convert.ToString(connectionID);
                }
            }

            return null;
        }
    }
}
