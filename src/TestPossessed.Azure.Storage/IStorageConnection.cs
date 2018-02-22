namespace TestPossessed.Azure.Storage
{
    public interface IStorageConnection
    {
        string CopyBlockBlob(string sourceContainerName,
            string sourceKey,
            string targetContainerName,
            string targetKey,
            bool allowDownload = false);

        IBlockBlobReader CreateBlockBlobReader(string containerName);
        IBlockBlobWriter CreateBlockBlobWriter(string containerName);
        IQueueReader CreateQueueReader(string queueName);
        IQueueWriter CreateQueueWriter(string queueName);
        IStorageConnection Open(string connectionstringName);
    }
}