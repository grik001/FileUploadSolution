using Autofac;
using Common.Helpers;
using Common.Helpers.IHelpers;
using Data.DataModels;
using Entities;
using FileUpload.Service.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Service
{
    class Program
    {
        static IApplicationConfig _applicationConfig = null;
        static IMessageQueueHelper _messageQueueHelper = null;
        static ICacheHelper _cacheHelper = null;
        static IFileDataModel _fileDataModel = null;
        static ILogger _logger = null;
        static IFileUploadHelper _fileUploadHelper = null;

        static void Main(string[] args)
        {
            #region DI Container Builder
            var builder = new ContainerBuilder();

            builder.RegisterType<RedisHelper>().As<ICacheHelper>();
            builder.RegisterType<ApplicationConfig>().As<IApplicationConfig>();
            builder.RegisterType<RabbitMQHelper>().As<IMessageQueueHelper>();
            builder.RegisterType<FileDataModel>().As<IFileDataModel>();
            builder.RegisterType<Log4NetHelper>().As<ILogger>();
            builder.RegisterType<AzureBlobStorageHelper>().As<IFileUploadHelper>();


            var container = builder.Build();
            #endregion

            using (var scope = container.BeginLifetimeScope())
            {
                #region DI Resolver
                _logger = scope.Resolve<ILogger>();
                _cacheHelper = scope.Resolve<ICacheHelper>();
                _messageQueueHelper = scope.Resolve<IMessageQueueHelper>();
                _applicationConfig = scope.Resolve<IApplicationConfig>();
                _fileDataModel = scope.Resolve<IFileDataModel>();
                _fileUploadHelper = scope.Resolve<IFileUploadHelper>();
                #endregion

                var fileProcessor = new FileProcessor(_fileDataModel, _fileUploadHelper, _applicationConfig);

                #region Processors
                Task.Run(() => _messageQueueHelper.ReadMessages<FileMetaData>(_applicationConfig, fileProcessor.FilePushed, _applicationConfig.FileDataCreateQueue));
                Task.Run(() => _messageQueueHelper.ReadMessages<FileMetaData>(_applicationConfig, fileProcessor.FileDelete, _applicationConfig.FileMetaDeleteQueue));
                Task.Run(() => _messageQueueHelper.ReadMessages<FileMetaData>(_applicationConfig, fileProcessor.FileOpened, _applicationConfig.FileOpenedQueue));
                #endregion
            }

            Console.ReadLine();
        }


    }
}