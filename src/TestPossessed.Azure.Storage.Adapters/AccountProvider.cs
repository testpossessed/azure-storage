using Microsoft.WindowsAzure.Storage;

namespace TestPossessed.Azure.Storage.Adapters
{
    public class AccountProvider : IAccountProvider
    {
        public IStorageAccount Open(string connectionString)
        {
            return new StorageAccount(CloudStorageAccount.Parse(connectionString));
        }
    }
}