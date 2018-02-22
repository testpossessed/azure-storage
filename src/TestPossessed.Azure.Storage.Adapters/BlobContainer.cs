using Microsoft.WindowsAzure.Storage.Blob;

namespace TestPossessed.Azure.Storage.Adapters
{
    public class BlobContainer : IBlobContainer
    {
        private readonly CloudBlobContainer cloudBlobContainer;

        public BlobContainer(CloudBlobContainer cloudBlobContainer)
        {
            this.cloudBlobContainer = cloudBlobContainer;
        }

        public void CreateIfNotExists()
        {
            this.cloudBlobContainer.CreateIfNotExists();
        }
        
        public void CreateWithPublicAccessIfNotExists()
        {
            this.cloudBlobContainer.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
        }

        public IBlockBlob GetBlockBlob(string key)
        {
            var blockBlob = this.cloudBlobContainer.GetBlockBlobReference(key);
            return blockBlob == null? null: new BlockBlob(blockBlob);
        }
    }
}