using System.IO;

namespace TestPossessed.Azure.Storage.Tests
{
    [TestFixture]
    public class BlockBlobReaderShould : MetricConsumerTestBase<IBlockBlobReader>
    {
        private const string Key = "key";

        [Fact]
        public void UseFactoryToCreateMetric()
        {
            this.AssumeReadTextWasRequested();
            this.AssertFactoryWasUsedToCreateMetric();
        }

        [Fact]
        public void StartMetric()
        {
            this.AssumeReadTextWasRequested();
            this.AssertMetricWasStartedWith("ReadText");
        }

        [Fact]
        public void UseContainerToCreateBlob()
        {
            this.AssumeReadTextWasRequested();
            this.blobContainer.Received()
                .GetBlockBlob(Key);
        }

        [Fact]
        public void UseBlobToDownloadContent()
        {
            this.AssumeReadTextWasRequested();
            this.blockBlob.Received().DownloadText();
        }

        [Fact]
        public void NotAttemptToDownloadContentIfBlobDoesNotExist()
        {
            this.blockBlob.Exists()
                .Returns(false);
            this.AssumeReadTextWasRequested();
            this.blockBlob.DidNotReceive()
                .DownloadText();
        }

        [Fact]
        public void ReturnNullIfBlobDoesNotExist()
        {
            this.blockBlob.Exists()
                .Returns(false);
            this.target.ReadText(Key)
                .Should()
                .BeNull();
        }

        private Stream stream;
        private IBlobContainer blobContainer;
        private IBlockBlob blockBlob;

        protected override IBlockBlobReader CreateTestTarget(ILogWriter logWriter, IMetricFactory metricFactory)
        {
            this.AssumeStreamIsInitialised();
            return new BlockBlobReader(logWriter, metricFactory, this.blobContainer);
        }

        private void AssumeStreamIsInitialised()
        {
            this.blobContainer = Substitute.For<IBlobContainer>();
            this.blockBlob = Substitute.For<IBlockBlob>();
            this.blockBlob.Exists()
                .Returns(true);
            this.blobContainer.GetBlockBlob(Key)
                .Returns(this.blockBlob);
            this.stream = new MemoryStream();
        }

        private void AssumeReadTextWasRequested()
        {
            this.target.ReadText(Key);
        }
    }
}