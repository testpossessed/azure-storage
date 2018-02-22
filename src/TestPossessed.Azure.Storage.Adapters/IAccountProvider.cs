namespace TestPossessed.Azure.Storage.Adapters
{
    public interface IAccountProvider
    {
        IStorageAccount Open(string connectionString);
    }
}