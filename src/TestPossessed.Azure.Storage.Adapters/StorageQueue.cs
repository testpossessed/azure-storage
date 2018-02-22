using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace TestPossessed.Azure.Storage.Adapters
{
    public class StorageQueue : IStorageQueue
    {
        private readonly CloudQueue cloudQueue;

        public StorageQueue(CloudQueue cloudQueue)
        {
            this.cloudQueue = cloudQueue;
        }

        public void AddMessage(IQueueMessage message)
        {
            var queueMessage = message as QueueMessage;
            if(queueMessage != null)
            {
                this.cloudQueue.AddMessageAsync(queueMessage.CloudQueueMessage);
            }
        }

        public void CreateIfNotExist()
        {
            this.cloudQueue.CreateIfNotExistsAsync();
        }

        public void DeleteMessage(IQueueMessage message)
        {
            var queueMessage = message as QueueMessage;
            if(queueMessage != null)
            {
                this.cloudQueue.DeleteMessageAsync(queueMessage.CloudQueueMessage);
            }
        }

        public IQueueMessage GetMessage(TimeSpan timeOut)
        {
            var message = this.cloudQueue.GetMessagesAsync(timeOut);
            return message == null? null: new QueueMessage(message);
        }
    }
}