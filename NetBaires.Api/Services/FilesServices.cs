using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Options;

namespace NetBaires.Api.Services
{
    public class FilesServices : IFilesServices
    {
        private readonly ConnectionStringsOptions connectionStringsOptions;
        private readonly ILogger<FilesServices> _logger;

        public FilesServices(IOptions<ConnectionStringsOptions> ConnectionStringsOptions,
        ILogger<FilesServices> logger)
        {
            connectionStringsOptions = ConnectionStringsOptions.Value;
            this._logger = logger;
        }
        public async Task<FileDetail> UploadAsync(Stream file, string fileName, Container container)
        {
            CloudStorageAccount storageAccount;
            CloudStorageAccount.TryParse(connectionStringsOptions.BlobStorage, out storageAccount);

            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer =
                cloudBlobClient.GetContainerReference(container.ToString().ToLower());
            await cloudBlobContainer.CreateIfNotExistsAsync();

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            await cloudBlockBlob.UploadFromStreamAsync(file);


            return new FileDetail(fileName);
        }
        public async Task<bool> DeleteAsync(string fileName, Container container)
        {

            try
            {
                CloudStorageAccount storageAccount;
                CloudStorageAccount.TryParse(connectionStringsOptions.BlobStorage, out storageAccount);

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
            CloudStorageAccount.TryParse(connectionStringsOptions.BlobStorage, out storageAccount);

            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer =
                    cloudBlobClient.GetContainerReference(container.ToString().ToLower());
            await cloudBlobContainer.CreateIfNotExistsAsync();

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            var blob = cloudBlobContainer.GetBlockBlobReference(fileName);
            Stream blobStream = await blob.OpenReadAsync();
            return blobStream;


        }
    }
}
