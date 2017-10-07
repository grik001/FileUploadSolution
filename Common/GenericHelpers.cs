﻿using System;
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
            var userID = HttpContext.Current.Request.Cookies["UserID"].Value;

            if (userID != null)
            {
                return Convert.ToString(userID);
            }

            return null;
        }

        public static string GetCurrentSocketID()
        {
            var connectionID = HttpContext.Current.Request.Cookies["ConnectionID"].Value;

            if (connectionID != null)
            {
                return Convert.ToString(connectionID);
            }

            return null;
        }
    }
}
