using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataModels
{
    public class AspNetUserDataModel : IAspNetUserDataModel
    {
        public AspNetUser Get(string email)
        {
            using (var context = new FileUploadEntities())
            {
                return context.AspNetUsers.FirstOrDefault(x => x.Email == email);
            }
        }
    }
}
