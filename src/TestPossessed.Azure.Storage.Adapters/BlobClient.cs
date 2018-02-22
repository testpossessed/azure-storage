using System;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace TestPossessed.Azure.Storage.Adapters
{
    public class BlobClient : IBlobClient
    {
        private readonly CloudBlobClient cloudBlobClient;

        public BlobClient(CloudBlobClient cloudBlobClient)
        {
            this.cloudBlobClient = cloudBlobClient;
            this.cloudBlobClient.DefaultRequestOptions.RetryPolicy =
                new ExponentialRetry(TimeSpan.FromSeconds(2), 10);
        }

        public IBlobContainer GetContainer(string name)
        {
            return new BlobContainer(this.cloudBlobClient.GetContainerReference(name));
        }

        public void SetRetryPolicy(IRetryPolicy retryPolicy)
        {
            this.cloudBlobClient.DefaultRequestOptions.RetryPolicy = retryPolicy;
        }
    }
}