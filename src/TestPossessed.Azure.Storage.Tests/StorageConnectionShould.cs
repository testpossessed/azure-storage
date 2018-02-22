using System;
using FluentAssertions;
using NSubstitute;
using TestPossessed.Azure.Storage.Adapters;
using Xunit;

namespace TestPossessed.Azure.Storage.Tests
{
    public class StorageConnectionShould
    {
        [Fact]
        public void QueryProviderForConnectionStringOnOpen()
        {
            this.AssumeOpenIsRequested();
            this.connectionStringProvider.Received()
                .Get(ConnectionstringName);
        }

        [Fact]
        public void UseProviderToOpenStorageAccountOnOpen()
        {
            this.AssumeOpenIsRequested();
            this.accountProvider.Received()
                .Open(ConnectionString);
        }

        [Fact]
        public void ReturnStorageConnectionOnOpen()
        {
            var connection = this.target.Open(ConnectionstringName);
            connection.Should()
                      .BeAssignableTo<IStorageConnection>();
            connection.Should()
                      .BeOfType<StorageConnection>();
        }

       [Fact]
        public void UseStorageAccountToCreateQueueClientForReader()
        {
            this.AssumeCreateQueueReaderIsRequested();
            this.storageAccount.Received()
                .CreateQueueClient();
        }

        [Fact]
        public void UseQueueClientToOpenQueueForReader()
        {
            this.AssumeCreateQueueReaderIsRequested();
            this.queueClient.Received()
                .GetQueue(QueueName);
        }

        [Fact]
        public void UseQueueToCreateQueueForReaderIfItDoesNotExist()
        {
            this.AssumeCreateQueueReaderIsRequested();
            this.queue.Received()
                .CreateIfNotExist();
        }

        [Fact]
        public void ReturnQueueReader()
        {
            var actual = this.AssumeCreateQueueReaderIsRequested();
            actual.Should()
                  .BeAssignableTo<IQueueReader>();
            actual.Should()
                  .BeOfType<QueueReader>();
        }

        [Fact]
        public void UseStorageAccountToCreateQueueClientForWriter()
        {
            this.AssumeCreateQueueWriterIsRequested();
            this.storageAccount.Received()
                .CreateQueueClient();
        }

        [Fact]
        public void UseQueueClientToOpenQueueForWriter()
        {
            this.AssumeCreateQueueWriterIsRequested();
            this.queueClient.Received()
                .GetQueue(QueueName);
        }

        [Fact]
        public void UseQueueToCreateQueueForWriterIfItDoesNotExist()
        {
            this.AssumeCreateQueueWriterIsRequested();
            this.queue.Received()
                .CreateIfNotExist();
        }

        [Fact]
        public void ReturnQueueWriter()
        {
            var actual = this.AssumeCreateQueueWriterIsRequested();
            actual.Should()
                  .BeAssignableTo<IQueueWriter>();
            actual.Should()
                  .BeOfType<QueueWriter>();
        }
        
        [Fact]
        public void UseStorageAccountToCreateBlobClientForWriter()
        {
            this.AssumeCreateBlobWriterIsRequested();
            this.storageAccount.Received()
                .CreateBlobClient();
        }

        [Fact]
        public void UseBlobClientToOpenContainerForWriter()
        {
            this.AssumeCreateBlobWriterIsRequested();
            this.blobClient.Received()
                .GetContainer(ContainerName);
        }

        [Fact]
        public void CreateContainerForWriterIfItDoesNotExist()
        {
            this.AssumeCreateBlobWriterIsRequested();
            this.blobContainer.Received()
                .CreateIfNotExists();
        }

        [Fact]
        public void ReturnBlobWriter()
        {
            var actual = this.AssumeCreateBlobWriterIsRequested();
            actual.Should()
                  .BeAssignableTo<IBlockBlobWriter>();
            actual.Should()
                  .BeOfType<BlockBlobWriter>();
        }
        
        [Fact]
        public void UseStorageAccountToCreateBlobClientForReader()
        {
            this.AssumeCreateBlobReaderIsRequested();
            this.storageAccount.Received()
                .CreateBlobClient();
        }

        [Fact]
        public void UseBlobClientToOpenContainerForReader()
        {
            this.AssumeCreateBlobReaderIsRequested();
            this.blobClient.Received()
                .GetContainer(ContainerName);
        }

        [Fact]
        public void CreateContainerForReaderIfItDoesNotExist()
        {
            this.AssumeCreateBlobReaderIsRequested();
            this.blobContainer.Received()
                .CreateIfNotExists();
        }

        [Fact]
        public void ReturnBlobReader()
        {
            var actual = this.AssumeCreateBlobReaderIsRequested();
            actual.Should()
                  .BeAssignableTo<IBlockBlobReader>();
            actual.Should()
                  .BeOfType<BlockBlobReader>();
        }

        [Fact]
        public void UseStorageAccountToCreateBlobClient()
        {
            this.AssumeCopyBlockBlobIsRequested();
            this.storageAccount.Received()
                .CreateBlobClient();
        }

        [Fact]
        public void UseBlobClientToGetSourceContainerOnCopyBlockBlob()
        {
            this.AssumeCopyBlockBlobIsRequested();
            this.blobClient.Received()
                .GetContainer(SourceContainer);
        }

        [Fact]
        public void EnsureSourceContainerExistsOnCopyBlockBlob()
        {
            this.AssumeCopyBlockBlobIsRequested();
            this.blobContainer.Received().CreateIfNotExists();
        }

        [Fact]
        public void UseBlobClientToGetTargetContainerOnCopyBlockBlob()
        {
            this.AssumeCopyBlockBlobIsRequested();
            this.blobClient.Received()
                .GetContainer(TargetContainer);
        }
        
        [Fact]
        public void EnsureTargetContainerExistsOnCopyBlockBlob()
        {
            this.AssumeCopyBlockBlobIsRequested();
            this.blobContainer.Received(2).CreateIfNotExists();
        }

        [Fact]
        public void UseContainerToGetReferenceToSourceOnCopyBlob()
        {
            this.AssumeCopyBlockBlobIsRequested();
            this.blobContainer.Received()
                .GetBlockBlob(SourceKey);
        }

        [Fact]
        public void UseContainerToGetReferenceToTargetOnCopyBlob()
        {
            this.AssumeCopyBlockBlobIsRequested();
            this.blobContainer.Received()
                .GetBlockBlob(TargetKey);
        }

        [Fact]
        public void StartCopyOnTargetBlob()
        {
            this.AssumeCopyBlockBlobIsRequested();
            this.blockBlob.Received()
                .StartCopy(this.blockBlob);
        }

        [Fact]
        public void CreateContainerWithPublicAccessIfDownloadAllowed()
        {
            this.AssumeCopyBlockBlobIsRequested(true);
            this.blobContainer.Received().CreateWithPublicAccessIfNotExists();;
        }

        private const string ConnectionString = "ConnectionString";
        private const string ConnectionstringName = "PcgStorageAccount";
        private const string ContainerName = "Container";
        private const string QueueName = "Queue";
        private const string SourceContainer = "SourceContainer";
        private const string SourceKey = "SourceKey";
        private const string TargetContainer = "TargetContainer";
        private const string TargetKey = "TargetKey";
        private IAccountProvider accountProvider;
        private IAppSettingProvider appSettingProvider;
        private IBlobClient blobClient;
        private IBlobContainer blobContainer;
        private IBlockBlob blockBlob;
        private IConnectionStringProvider connectionStringProvider;
        private IStorageQueue queue;
        private IQueueClient queueClient;
        private IStorageAccount storageAccount;
        private StorageConnection target;

        public StorageConnectionShould()
        {
            this.AssumeConnectionStringProviderIsInitialised();
            this.AssumeAppSettingProviderIsInitialised();
            this.AssumeAccountProviderIsInitialised();
            this.target = new StorageConnection(this.connectionStringProvider,
                this.appSettingProvider,
                this.accountProvider);
        }

        private void AssumeAccountProviderIsInitialised()
        {
            this.accountProvider = Substitute.For<IAccountProvider>();
            this.storageAccount = Substitute.For<IStorageAccount>();
            this.queueClient = Substitute.For<IQueueClient>();
            this.queue = Substitute.For<IStorageQueue>();
            this.accountProvider.Open(Arg.Any<string>())
                .Returns(this.storageAccount);
            this.storageAccount.CreateQueueClient()
                .Returns(this.queueClient);
            this.queueClient.GetQueue(Arg.Any<string>())
                .Returns(this.queue);
            this.blobClient = Substitute.For<IBlobClient>();
            this.storageAccount.CreateBlobClient()
                .Returns(this.blobClient);
            this.blobContainer = Substitute.For<IBlobContainer>();
            this.blobClient.GetContainer(Arg.Any<string>())
                .Returns(this.blobContainer);
            this.blockBlob = Substitute.For<IBlockBlob>();
            this.blobContainer.GetBlockBlob(Arg.Any<string>())
                .Returns(this.blockBlob);
            this.blockBlob.Uri.Returns(new Uri("http://localhost/8056/abcd12345"));
        }

        private void AssumeAppSettingProviderIsInitialised()
        {
            this.appSettingProvider = Substitute.For<IAppSettingProvider>();
        }

        private void AssumeConnectionStringProviderIsInitialised()
        {
            this.connectionStringProvider = Substitute.For<IConnectionStringProvider>();
            this.connectionStringProvider.Get(ConnectionstringName)
                .Returns(ConnectionString);
        }

        private void AssumeCopyBlockBlobIsRequested(bool isDownloadAllowed = false)
        {
            this.AssumeOpenIsRequested();
            this.target.CopyBlockBlob(SourceContainer, SourceKey, TargetContainer, TargetKey, isDownloadAllowed);
        }

        private IBlockBlobReader AssumeCreateBlobReaderIsRequested()
        {
            this.AssumeOpenIsRequested();
            return this.target.CreateBlockBlobReader(ContainerName);
        }

        private IBlockBlobWriter AssumeCreateBlobWriterIsRequested()
        {
            this.AssumeOpenIsRequested();
            return this.target.CreateBlockBlobWriter(ContainerName);
        }

        private IQueueReader AssumeCreateQueueReaderIsRequested()
        {
            this.AssumeOpenIsRequested();
            return this.target.CreateQueueReader(QueueName);
        }

        private IQueueWriter AssumeCreateQueueWriterIsRequested()
        {
            this.AssumeOpenIsRequested();
            return this.target.CreateQueueWriter(QueueName);
        }

        private void AssumeOpenIsRequested()
        {
            this.target.Open(ConnectionstringName);
        }
    }
}