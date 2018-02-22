namespace TestPossessed.Azure.Storage.Adapters
{
    public interface IStorageAccount
    {
        IQueueClient CreateQueueClient();
        IBlobClient CreateBlobClient();
    }
}