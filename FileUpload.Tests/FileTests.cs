using Common;
using Common.Helpers;
using Common.Helpers.IHelpers;
using Data.DataModels;
using Entities;
using Entities.ViewModels;
using FileUpload.Front.Controllers;
using FileUpload.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace FileUpload.Tests
{
    [TestClass]
    public class FileTests
    {
        Mock<IFileDataModel> _MockFileDataModel = new Mock<IFileDataModel>();
        Mock<IMessageQueueHelper> _MockMessageQueueHelper = new Mock<IMessageQueueHelper>();
        Mock<ILogger> _MockLogger = new Mock<ILogger>();
        Mock<IApplicationConfig> _MockAppConfig = new Mock<IApplicationConfig>();
        Mock<IFileUploadHelper> _MockFileUploadHelper = new Mock<IFileUploadHelper>();
        Mock<IGenericHelper> _MockGenericHelper = new Mock<IGenericHelper>();

        #region File/Get

        [TestMethod]
        public void File_Get_ValidFileData()
        {
            _MockFileDataModel.Setup(x => x.Get(It.IsAny<string>())).Returns(FileData.FilesValid);
            _MockGenericHelper.Setup(x => x.GetUserID()).Returns(Guid.NewGuid().ToString());

            var model = new FileController(_MockFileDataModel.Object, _MockLogger.Object, _MockMessageQueueHelper.Object, _MockAppConfig.Object, _MockFileUploadHelper.Object, _MockGenericHelper.Object);

            var result = model.Get() as OkNegotiatedContentResult<FileViewModel[]>;
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<FileViewModel[]>));
            Assert.IsTrue(result.Content.Any());
        }

        [TestMethod]
        public void File_Get_InValidFileDataShouldNotBreak()
        {
            _MockFileDataModel.Setup(x => x.Get(It.IsAny<string>())).Returns(FileData.FilesInvalid);
            _MockGenericHelper.Setup(x => x.GetUserID()).Returns(Guid.NewGuid().ToString());

            var model = new FileController(_MockFileDataModel.Object, _MockLogger.Object, _MockMessageQueueHelper.Object, _MockAppConfig.Object, _MockFileUploadHelper.Object, _MockGenericHelper.Object);

            var result = model.Get() as OkNegotiatedContentResult<FileViewModel[]>;
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<FileViewModel[]>));
        }

        [TestMethod]
        public void File_Get_InValidFileDataReturnNullShouldReturnEmpty()
        {
            _MockFileDataModel.Setup(x => x.Get(It.IsAny<string>())).Returns((List<FileMetaData>)null);
            _MockGenericHelper.Setup(x => x.GetUserID()).Returns(Guid.NewGuid().ToString());

            var model = new FileController(_MockFileDataModel.Object, _MockLogger.Object, _MockMessageQueueHelper.Object, _MockAppConfig.Object, _MockFileUploadHelper.Object, _MockGenericHelper.Object);

            var result = model.Get() as OkNegotiatedContentResult<FileViewModel[]>;
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<FileViewModel[]>));
        }

        [TestMethod]
        public void File_Get_InValidUserIsNull()
        {
            _MockGenericHelper.Setup(x => x.GetUserID()).Returns((string)null);

            var model = new FileController(_MockFileDataModel.Object, _MockLogger.Object, _MockMessageQueueHelper.Object, _MockAppConfig.Object, _MockFileUploadHelper.Object, _MockGenericHelper.Object);

            var result = model.Get();
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        #endregion

        #region File/Post
        [TestMethod]
        public void File_Post_Valid()
        {
            _MockGenericHelper.Setup(x => x.GetFilesFromHttpMessage()).Returns(TestData.FileData.filesUploadValid);
            _MockGenericHelper.Setup(x => x.GetUserID()).Returns(Guid.NewGuid().ToString());
            _MockGenericHelper.Setup(x => x.IsFileAccepted(It.IsAny<IApplicationConfig>(), It.IsAny<string>())).Returns(true);
            _MockMessageQueueHelper.Setup(x => x.PushMessage<FileMetaData>(It.IsAny<IApplicationConfig>(), It.IsAny<FileMetaData>(), It.IsAny<string>()));
            _MockFileUploadHelper.Setup(x => x.UploadFile(It.IsAny<IApplicationConfig>(), It.IsAny<Stream>(), It.IsAny<string>())).Returns("www.test.com/test.csv");


            var model = new FileController(_MockFileDataModel.Object, _MockLogger.Object, _MockMessageQueueHelper.Object, _MockAppConfig.Object, _MockFileUploadHelper.Object, _MockGenericHelper.Object);

            var result = model.Post() as OkNegotiatedContentResult<List<FileViewModel>>;
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<FileViewModel>>));
            Assert.IsTrue(result.Content.Any());
        }

        [TestMethod]
        public void File_Post_InValid()
        {
            _MockGenericHelper.Setup(x => x.GetFilesFromHttpMessage()).Returns(TestData.FileData.filesUploadInValid);
            _MockGenericHelper.Setup(x => x.GetUserID()).Returns(Guid.NewGuid().ToString());
            _MockGenericHelper.Setup(x => x.IsFileAccepted(It.IsAny<IApplicationConfig>(), It.IsAny<string>())).Returns(false);
            _MockMessageQueueHelper.Setup(x => x.PushMessage<FileMetaData>(It.IsAny<IApplicationConfig>(), It.IsAny<FileMetaData>(), It.IsAny<string>()));
            _MockFileUploadHelper.Setup(x => x.UploadFile(It.IsAny<IApplicationConfig>(), It.IsAny<Stream>(), It.IsAny<string>())).Returns("www.test.com/test.csv");
            var model = new FileController(_MockFileDataModel.Object, _MockLogger.Object, _MockMessageQueueHelper.Object, _MockAppConfig.Object, _MockFileUploadHelper.Object, _MockGenericHelper.Object);

            var result = model.Post() as OkNegotiatedContentResult<List<FileViewModel>>;
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<FileViewModel>>));
            Assert.IsFalse(result.Content.Any());
        }
        #endregion

        #region File/Put
        [TestMethod]
        public void File_Put_Valid()
        {
            _MockGenericHelper.Setup(x => x.GetUserID()).Returns(Guid.NewGuid().ToString());
            _MockMessageQueueHelper.Setup(x => x.PushMessage<FileMetaData>(It.IsAny<IApplicationConfig>(), It.IsAny<FileMetaData>(), It.IsAny<string>()));

            var model = new FileController(_MockFileDataModel.Object, _MockLogger.Object, _MockMessageQueueHelper.Object, _MockAppConfig.Object, _MockFileUploadHelper.Object, _MockGenericHelper.Object);

            var result = model.Put(Guid.NewGuid()) as OkResult;
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void File_Put_InValid()
        {
            _MockGenericHelper.Setup(x => x.GetUserID()).Returns((string)null);
            var model = new FileController(_MockFileDataModel.Object, _MockLogger.Object, _MockMessageQueueHelper.Object, _MockAppConfig.Object, _MockFileUploadHelper.Object, _MockGenericHelper.Object);

            var result = model.Put(Guid.NewGuid()) as NotFoundResult;
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        #endregion

        #region File/Delete
        [TestMethod]
        public void File_Delete_Valid()
        {
            _MockGenericHelper.Setup(x => x.GetUserID()).Returns(Guid.NewGuid().ToString());
            _MockMessageQueueHelper.Setup(x => x.PushMessage<FileMetaData>(It.IsAny<IApplicationConfig>(), It.IsAny<FileMetaData>(), It.IsAny<string>()));

            var model = new FileController(_MockFileDataModel.Object, _MockLogger.Object, _MockMessageQueueHelper.Object, _MockAppConfig.Object, _MockFileUploadHelper.Object, _MockGenericHelper.Object);

            var result = model.Delete(Guid.NewGuid()) as OkResult;
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void File_Delete_InValid()
        {
            _MockGenericHelper.Setup(x => x.GetUserID()).Returns((string)null);
            var model = new FileController(_MockFileDataModel.Object, _MockLogger.Object, _MockMessageQueueHelper.Object, _MockAppConfig.Object, _MockFileUploadHelper.Object, _MockGenericHelper.Object);

            var result = model.Delete(Guid.NewGuid()) as NotFoundResult;
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        #endregion

        #region ExtensionChecks
        [TestMethod]
        public void IsFileAccepted_Test()
        {
            _MockAppConfig.Setup(x => x.AcceptedFiles).Returns(".csv,.xlsx");

            GenericHelpers genericHelper = new GenericHelpers();

            Assert.IsTrue(genericHelper.IsFileAccepted(_MockAppConfig.Object, ".csv"));
            Assert.IsTrue(genericHelper.IsFileAccepted(_MockAppConfig.Object, ".xlsx"));
            Assert.IsFalse(genericHelper.IsFileAccepted(_MockAppConfig.Object, ".jpg"));
            Assert.IsFalse(genericHelper.IsFileAccepted(_MockAppConfig.Object, ".txt"));
            Assert.IsFalse(genericHelper.IsFileAccepted(_MockAppConfig.Object, ".png"));
        }
        #endregion
    }
}
