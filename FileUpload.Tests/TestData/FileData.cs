using Common.Helpers;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Tests.TestData
{
    public class FileData
    {
        public static List<FileMetaData> FilesValid = new List<FileMetaData>()
        {
            new FileMetaData(){ ID = Guid.NewGuid(), BlobUrl = "http://test.com/file.csv", FileExtension = ".csv", Filename = "file", FileSize = 12501, UserID = Guid.NewGuid().ToString(), ViewCount = 999 },
            new FileMetaData(){ ID = Guid.NewGuid(), BlobUrl = "http://test.com/file.csv", FileExtension = ".csv", Filename = "file", FileSize = 12501, UserID = Guid.NewGuid().ToString(), ViewCount = 999 }
        };

        public static List<FileMetaData> FilesInvalid = new List<FileMetaData>()
        {
            new FileMetaData(){ ID = Guid.Empty, BlobUrl = null, FileExtension = null, Filename = null, FileSize = null, UserID = null, ViewCount = 0 },
        };

        public static List<HttpFileUploadHelper> filesUploadValid = new List<HttpFileUploadHelper>()
        {
            new HttpFileUploadHelper(){ ContentLength = 100, ContentType = "text/csv", FileKey = Guid.NewGuid().ToString(), FileName = "test.csv", InputStream = null  },
            new HttpFileUploadHelper(){ ContentLength = 300, ContentType = "text/csv", FileKey = Guid.NewGuid().ToString(), FileName = "test2.csv", InputStream = null  }
        };

        public static List<HttpFileUploadHelper> filesUploadInValid = new List<HttpFileUploadHelper>()
        {
            new HttpFileUploadHelper(){ ContentLength = 100, ContentType = "text/csv", FileKey = Guid.NewGuid().ToString(), FileName = "test.jpg", InputStream = null  },
            new HttpFileUploadHelper(){ ContentLength = 300, ContentType = "jpeg", FileKey = Guid.NewGuid().ToString(), FileName = "test2.csv", InputStream = null  }
        };
    }
}
