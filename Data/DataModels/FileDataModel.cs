using Common.Helpers.IHelpers;
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
        ICacheHelper _cacheHelper = null;
        IApplicationConfig _applicationConfig = null;

        public FileDataModel(ICacheHelper cacheHelper, IApplicationConfig applicationConfig)
        {
            this._cacheHelper = cacheHelper;
            this._applicationConfig = applicationConfig;
        }

        public List<FileMetaData> Get()
        {
            var files = _cacheHelper.GetValue<List<FileMetaData>>(_applicationConfig.RedisFileMetaList);

            if (files == null)
            {
                using (var context = new FileUploadEntities())
                {
                    files = context.FileMetaDatas.ToList();
                    _cacheHelper.SetValue<List<FileMetaData>>(_applicationConfig.RedisFileMetaList, files);
                }
            }

            return files;
        }

        public List<FileMetaData> Get(string userID)
        {
            var files = _cacheHelper.GetValue<List<FileMetaData>>(_applicationConfig.RedisFileMetaList);

            if (files == null)
            {
                using (var context = new FileUploadEntities())
                {
                    files = context.FileMetaDatas.ToList();
                    _cacheHelper.SetValue<List<FileMetaData>>(_applicationConfig.RedisFileMetaList, files);

                }
            }

            files = files.Where(x => x.UserID == userID).ToList();
            return files;
        }

        public FileMetaData Get(Guid id)
        {
            var files = _cacheHelper.GetValue<List<FileMetaData>>(_applicationConfig.RedisFileMetaList);

            if (files == null)
            {
                using (var context = new FileUploadEntities())
                {
                    files = context.FileMetaDatas.ToList();
                    _cacheHelper.SetValue<List<FileMetaData>>(_applicationConfig.RedisFileMetaList, files);
                }
            }

            var file = files.FirstOrDefault(x => x.ID == id);
            return file;
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

                _cacheHelper.SetValue<List<FileMetaData>>(_applicationConfig.RedisFileMetaList, null);

                return file;
            }
        }

        public int Update(FileMetaData file)
        {
            using (var context = new FileUploadEntities())
            {
                context.Entry(file).State = EntityState.Modified;
                return context.SaveChanges();

                _cacheHelper.SetValue<List<FileMetaData>>(_applicationConfig.RedisFileMetaList, null);
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

                    _cacheHelper.SetValue<List<FileMetaData>>(_applicationConfig.RedisFileMetaList, null);

                    return true;
                }

                return false;
            }
        }
    }
}
