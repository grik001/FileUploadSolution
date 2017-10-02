using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataModels
{
    public interface IFileDataModel
    {
        List<File> Get();
        File Get(Guid id);
        List<File> Get(string userID);

        File Insert(File file);
        int Update(File file);
        bool Delete(Guid id);
    }
}
