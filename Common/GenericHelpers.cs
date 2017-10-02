using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public class GenericHelpers
    {
        public static string GetUserID()
        {
            return Convert.ToString(HttpContext.Current.Session["UserID"]);
        }
    }
}
