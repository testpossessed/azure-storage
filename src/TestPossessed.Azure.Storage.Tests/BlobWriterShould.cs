using System.IO;

namespace TestPossessed.Azure.Storage.Tests
{
    [TestFixture]
    public class BlobWriterShould : MetricConsumerTestBase<IBlockBlobWriter>
    {
        [Fact]
        public void UseFactoryToCreateMetricOnWriteStream()
        {
            this.AssumeWriteStreamWasRequested();
            this.AssertFactoryWasUsedToCreateMetric();
        }

        [Fact]
        public void StartMetricOnWriteStream()
        {
            this.AssumeWriteStreamWasRequested();
            this.AssertMetricWasStartedWith("Write");
        }

        [Fact]
        public void UseContainerToCreateBlobFromStream()
        {
            this.AssumeWriteStreamWasRequested();
            this.blobContainer.Received()
                .GetBlockBlob(Key);
        }

        [Fact]
        public void UseBlobToUploadContent()
        {
            this.AssumeWriteStreamWasRequested();
            this.blockBlob.Received()
                .UploadFromStream(this.stream);
        }

        [Fact]
        public void UseFactoryToCreateMetricOnWriteString()
        {
            this.AssumeWriteStringWasRequested();
            this.AssertFactoryWasUsedToCreateMetric();
        }

        [Fact]
        public void StartMetricOnWriteString()
        {
            this.AssumeWriteStringWasRequested();
            this.AssertMetricWasStartedWith("Write");
        }

        [Fact]
        public void ReturnBlobUriOnWriteStream()
        {
            this.AssumeWriteStreamWasRequested();
            var dummy = this.blockBlob.Received().Uri;
        }

        [Fact]
        public void ReturnBlobUriOnWriteSting()
        {
            this.AssumeWriteStringWasRequested();
            var dummy = this.blockBlob.Received().Uri;
        }

        private const string Key = "key";
        private IBlobContainer blobContainer;
        private IBlockBlob blockBlob;

        private Stream stream;

        protected override IBlockBlobWriter CreateTestTarget(ILogWriter logWriter,
            IMetricFactory metricFactory)
        {
            this.AssumeBlobContainerIsInitialised();
            return new BlockBlobWriter(logWriter, metricFactory, this.blobContainer);
        }

        private void AssumeBlobContainerIsInitialised()
        {
            this.blobContainer = Substitute.For<IBlobContainer>();
            this.blockBlob = Substitute.For<IBlockBlob>();
            this.blobContainer.GetBlockBlob(Key)
                .Returns(this.blockBlob);
        }

        private void AssumeWriteStreamWasRequested()
        {
            this.stream = new MemoryStream();
            this.target.Write(this.stream, Key);
        }

        private void AssumeWriteStringWasRequested()
        {
            this.target.Write("content", Key);
        }
    }
}