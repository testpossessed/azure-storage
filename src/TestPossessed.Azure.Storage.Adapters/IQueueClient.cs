using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace TestPossessed.Azure.Storage.Adapters
{
    public interface IQueueClient
    {
        IStorageQueue GetQueue(string name);
        void SetRetryPolicy(IRetryPolicy retryPolicy);
    }
}