using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers.IHelpers
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogWarn(string message, Exception ex);
        void LogError(string message, Exception ex);
    }
}
