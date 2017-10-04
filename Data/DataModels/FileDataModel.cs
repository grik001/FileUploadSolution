using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataModels
{
    public class FileDataModel : IFileDataModel
    {
        public List<FileMetaData> Get()
        {
            using (var context = new FileUploadEntities())
            {
                return context.FileMetaDatas.ToList();
            }
        }

        public List<FileMetaData> Get(string userID)
        {
            using (var context = new FileUploadEntities())
            {
                return context.FileMetaDatas.Where(x => x.UserID == userID).ToList();
            }
        }

        public FileMetaData Get(Guid id)
        {
            using (var context = new FileUploadEntities())
            {
                return context.FileMetaDatas.FirstOrDefault(x => x.ID == id);
            }
        }

        public bool Exists(string filename, string fileExtension)
        {
            using (var context = new FileUploadEntities())
            {
                var file = context.FileMetaDatas.FirstOrDefault(x => x.Filename == filename && x.FileExtension == fileExtension);
                return file == null ? false : true;
            }
        }

        public bool Exists(Guid id)
        {
            using (var context = new FileUploadEntities())
            {
                Guid? searchId = context.FileMetaDatas.Select(x => x.ID).FirstOrDefault(x => x == id);
                return searchId == null ? false : true;
            }
        }

        public FileMetaData Insert(FileMetaData file)
        {
            using (var context = new FileUploadEntities())
            {

                context.FileMetaDatas.Add(file);
                context.SaveChanges();
                return file;
            }
        }

        public int Update(FileMetaData file)
        {
            using (var context = new FileUploadEntities())
            {
                context.Entry(file).State = EntityState.Modified;
                return context.SaveChanges();
            }
        }

        public bool Delete(Guid id)
        {
            using (var context = new FileUploadEntities())
            {
                var value = context.FileMetaDatas.FirstOrDefault(x => x.ID == id);

                if (value != null)
                {
                    context.FileMetaDatas.Remove(value);
                    context.SaveChanges();
                    return true;
                }

                return false;
            }
        }
    }
}
