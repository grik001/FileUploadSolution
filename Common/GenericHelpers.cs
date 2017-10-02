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
            var session = HttpContext.Current.Session;

            if (session != null)
            {
                return Convert.ToString(session["UserID"]);
            }

#if DEBUG
            return "41e617de-f730-44bb-aab5-46193c4d4c52";
#endif

            return null;
        }
    }
}
