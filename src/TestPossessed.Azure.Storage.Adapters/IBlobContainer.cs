namespace TestPossessed.Azure.Storage.Adapters
{
    public interface IBlobContainer
    {
        void CreateIfNotExists();
        void CreateWithPublicAccessIfNotExists();
        IBlockBlob GetBlockBlob(string key);
    }
}