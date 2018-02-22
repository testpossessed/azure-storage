using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace TestPossessed.Azure.Storage.Adapters
{
    public interface IBlobClient
    {
        IBlobContainer GetContainer(string name);
        void SetRetryPolicy(IRetryPolicy retryPolicy);
    }
}