using System;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace TestPossessed.Azure.Storage.Adapters
{
    public class QueueClient : IQueueClient
    {
        private readonly CloudQueueClient cloudQueueClient;

        internal QueueClient(CloudQueueClient cloudQueueClient)
        {
            this.cloudQueueClient = cloudQueueClient;
            this.cloudQueueClient.DefaultRequestOptions.RetryPolicy =
                new ExponentialRetry(TimeSpan.FromSeconds(2), 10);
        }

        public IStorageQueue GetQueue(string name)
        {
            return new StorageQueue(this.cloudQueueClient.GetQueueReference(name));
        }

        public void SetRetryPolicy(IRetryPolicy retryPolicy)
        {
            this.cloudQueueClient.DefaultRequestOptions.RetryPolicy = retryPolicy;
        }
    }
}