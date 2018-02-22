using TestPossessed.Azure.Storage.Adapters;

namespace TestPossessed.Azure.Storage
{
    public class StorageConnection: IStorageConnection
    {
        private readonly IAccountProvider accountProvider;
        private IStorageAccount storageAccount;

        public StorageConnection(IAccountProvider accountProvider)
        {
            this.accountProvider = accountProvider;
        }

        public string CopyBlockBlob(string sourceContainerName,
            string sourceKey,
            string targetContainerName,
            string targetKey,
            bool allowDownload = false)
        {
            using(this.metricFactory.CreateLoggingTimerMetric(this.logWriter)
                      .Start(this.GetActionName("CopyBlockBlob")))
            {
                this.logWriter.Trace(
                    $"Opening containers {sourceContainerName} and {targetContainerName} for copy operation");
                var client = this.storageAccount.CreateBlobClient();
                var sourceContainerRef = client.GetContainer(sourceContainerName);
                var targetContainerRef = client.GetContainer(targetContainerName);
                sourceContainerRef.CreateIfNotExists();
                if(allowDownload)
                {
                    targetContainerRef.CreateWithPublicAccessIfNotExists();
                }
                else
                {
                    targetContainerRef.CreateIfNotExists();
                }
                var source = sourceContainerRef.GetBlockBlob(sourceKey);
                var target = targetContainerRef.GetBlockBlob(targetKey);
                target.StartCopy(source);
                return target.Uri.AbsoluteUri;
            }
        }

        public IBlockBlobReader CreateBlockBlobReader(string containerName)
        {
            using(this.metricFactory.CreateLoggingTimerMetric(this.logWriter)
                      .Start(this.GetActionName("CreateBlockBlobReader")))
            {
                this.logWriter.Trace($"Preparing container {containerName} for reading");
                var client = this.storageAccount.CreateBlobClient();
                var container = client.GetContainer(containerName);
                container.CreateIfNotExists();
                return new BlockBlobReader(this.logWriter, this.metricFactory, container);
            }
        }

        public IBlockBlobWriter CreateBlockBlobWriter(string containerName)
        {
            using(this.metricFactory.CreateLoggingTimerMetric(this.logWriter)
                      .Start(this.GetActionName("CreateBlockBlobWriter")))
            {
                this.logWriter.Trace($"Preparing container {containerName} for writing");
                var client = this.storageAccount.CreateBlobClient();
                var container = client.GetContainer(containerName);
                container.CreateIfNotExists();
                return new BlockBlobWriter(this.logWriter, this.metricFactory, container);
            }
        }

        public IQueueReader CreateQueueReader(string queueName)
        {
            using(this.metricFactory.CreateLoggingTimerMetric(this.logWriter)
                      .Start(this.GetActionName("CreateQueueReader")))
            {
                this.logWriter.Trace($"Preparing queue {queueName} for reading");
                var client = this.storageAccount.CreateQueueClient();
                var queue = client.GetQueue(queueName);
                queue.CreateIfNotExist();
                return new QueueReader(this.logWriter, this.metricFactory, this.appSettingProvider, queue);
            }
        }

        public IQueueWriter CreateQueueWriter(string queueName)
        {
            using(this.metricFactory.CreateLoggingTimerMetric(this.logWriter)
                      .Start(this.GetActionName("CreateQueueWriter")))
            {
                this.logWriter.Trace($"Preparing queue {queueName} for writing");
                var client = this.storageAccount.CreateQueueClient();
                var queue = client.GetQueue(queueName);
                queue.CreateIfNotExist();
                return new QueueWriter(this.logWriter, this.metricFactory, queue);
            }
        }

        public IStorageConnection Open(string connectionstringName)
        {
            using(this.metricFactory.CreateLoggingTimerMetric(this.logWriter)
                      .Start(this.GetActionName("Open")))
            {
                this.logWriter.Trace("Opening connection to storage account");
                this.storageAccount =
                    this.accountProvider.Open(this.connectionStringProvider.Get(connectionstringName));
                return this;
            }
        }
    }
}