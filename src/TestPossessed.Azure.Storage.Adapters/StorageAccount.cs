using Microsoft.WindowsAzure.Storage;

namespace TestPossessed.Azure.Storage.Adapters
{
    public sealed class StorageAccount : IStorageAccount
    {
        private readonly CloudStorageAccount storageAccount;

        internal StorageAccount(CloudStorageAccount storageAccount)
        {
            this.storageAccount = storageAccount;
        }

        public IQueueClient CreateQueueClient()
        {
            return new QueueClient(this.storageAccount.CreateCloudQueueClient());
        }

        public IBlobClient CreateBlobClient()
        {
            return new BlobClient(this.storageAccount.CreateCloudBlobClient());
        }
    }
}