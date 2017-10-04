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
        List<FileMetaData> Get();
        FileMetaData Get(Guid id);
        List<FileMetaData> Get(string userID);

        FileMetaData Insert(FileMetaData file);
        int Update(FileMetaData file);
        bool Delete(Guid id);
    }
}
