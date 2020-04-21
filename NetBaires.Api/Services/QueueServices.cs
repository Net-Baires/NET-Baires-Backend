using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Extensions.Options;
using NetBaires.Api.Options;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace NetBaires.Api.Services
{
    public class QueueServices : IQueueServices
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudQueueClient _queueClient;

        public QueueServices(IOptions<ConnectionStringsOptions> connectionStringsOptions)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionStringsOptions.Value.BlobStorage);
            _queueClient = _storageAccount.CreateCloudQueueClient();
        }

        public void AddMessage<TData>(TData data)
        {
            CloudQueue queue = _queueClient.GetQueueReference(data.GetType().Name.ToLower());
            queue.CreateIfNotExists();

            CloudQueueMessage message = new CloudQueueMessage(JsonConvert.SerializeObject(data));
            queue.AddMessage(message);
        }
        public TData GetMessage<TData>()
        {
            CloudQueue queue = _queueClient.GetQueueReference(typeof(TData).Name.ToLower());
            queue.CreateIfNotExists();
            var message = JsonConvert.DeserializeObject<TData>(queue.GetMessage().AsString);
            return message;
        }
        public void Clear<TData>()
        {
            CloudQueue queue = _queueClient.GetQueueReference(typeof(TData).Name.ToLower());
            queue.CreateIfNotExists();
            queue.Clear();
        }
    }

}
