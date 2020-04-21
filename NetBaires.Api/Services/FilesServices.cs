using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Services
{
    public class FilesServices : IFilesServices
    {
        private readonly ConnectionStringsOptions _connectionStringsOptions;
        private readonly ILogger<FilesServices> _logger;

        public FilesServices(IOptions<ConnectionStringsOptions> connectionStringsOptions,
        ILogger<FilesServices> logger)
        {
            _connectionStringsOptions = connectionStringsOptions.Value;
            _logger = logger;
        }
        public async Task<FileDetail> UploadAsync(Stream file, string fileName, Container container)
        {
            CloudStorageAccount.TryParse(_connectionStringsOptions.BlobStorage, out var storageAccount);

            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer =
                cloudBlobClient.GetContainerReference(container.ToString().ToLower());
            await cloudBlobContainer.CreateIfNotExistsAsync();

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            await cloudBlockBlob.UploadFromStreamAsync(file);


            return new FileDetail(fileName, cloudBlockBlob.Uri);
        }
        public async Task<bool> DeleteAsync(string fileName, Container container)
        {

            try
            {
                CloudStorageAccount storageAccount;
                CloudStorageAccount.TryParse(_connectionStringsOptions.BlobStorage, out storageAccount);

                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                CloudBlobContainer cloudBlobContainer =
                        cloudBlobClient.GetContainerReference(container.ToString().ToLower());
                await cloudBlobContainer.CreateIfNotExistsAsync();

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                var blob = cloudBlobContainer.GetBlockBlobReference(fileName);
                blob.DeleteIfExists();
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(FilesServices)}-DeleteAsync", e);
            }
            return true;

        }

        public async Task<Stream> GetAsync(string fileName, Container container)
        {
            CloudStorageAccount storageAccount;
            CloudStorageAccount.TryParse(_connectionStringsOptions.BlobStorage, out storageAccount);

            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer =
                    cloudBlobClient.GetContainerReference(container.ToString().ToLower());
            await cloudBlobContainer.CreateIfNotExistsAsync();

            try
            {
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                var blob = cloudBlobContainer.GetBlockBlobReference(fileName);
                Stream blobStream = await blob.OpenReadAsync();
                return blobStream;
            }
            catch (StorageException)
            {

                return null;
            }
          


        }
    }
}
