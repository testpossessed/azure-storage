namespace TestPossessed.Azure.Storage
{
    public class BlockBlobReader : IBlockBlobReader
    {
        private readonly IBlobContainer blobContainer;
        private readonly ILogWriter logWriter;
        private readonly IMetricFactory metricFactory;

        public BlockBlobReader(ILogWriter logWriter, IMetricFactory metricFactory, IBlobContainer blobContainer)
        {
            this.logWriter = logWriter;
            this.metricFactory = metricFactory;
            this.blobContainer = blobContainer;
        }

        public string ReadText(string key)
        {
            using(this.metricFactory.CreateLoggingTimerMetric(this.logWriter)
                      .Start("BlockBlobReader.ReadText"))
            {
                this.logWriter.Trace("Downloading text content of blob");
                var blob = this.blobContainer.GetBlockBlob(key);
                return blob.Exists() ? blob.DownloadText(): null;
            }
        }
    }
}