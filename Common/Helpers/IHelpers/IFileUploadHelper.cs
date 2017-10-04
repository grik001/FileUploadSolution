using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers.IHelpers
{
    public interface IFileUploadHelper
    {
        string UploadFile(IApplicationConfig config, Stream stream, string name);
    }
}
