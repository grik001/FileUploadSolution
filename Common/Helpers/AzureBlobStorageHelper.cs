using Common.Helpers.IHelpers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class AzureBlobStorageHelper : IFileUploadHelper
    {
        private ILogger _logger = null;

        public AzureBlobStorageHelper(IApplicationConfig config, ILogger logger)
        {
            this._logger = logger;
            CreateContainer(config);
        }

        public void CreateContainer(IApplicationConfig config)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config.BlobConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(config.CsvContainer);
                container.CreateIfNotExists();

                //TODO
                container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to create container in Azure Storage", ex);
            }
        }

        public string UploadFile(IApplicationConfig config, Stream stream, string reference)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config.BlobConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(config.CsvContainer);
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(reference);
                blockBlob.UploadFromStream(stream);
                return blockBlob.Uri.AbsoluteUri;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to upload file to Blob", ex);
                return null;
            }
        }


        public bool DeleteFile(IApplicationConfig config, string reference)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config.BlobConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(config.CsvContainer);
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(reference);
                return blockBlob.DeleteIfExists();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete file from Blob", ex);
                return false;
            }
        }

        public bool Exists(IApplicationConfig config, string reference)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config.BlobConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(config.CsvContainer);
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(reference);
                return blockBlob.Exists();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to check if file exists", ex);
                return false;
            }

        }
    }
}
