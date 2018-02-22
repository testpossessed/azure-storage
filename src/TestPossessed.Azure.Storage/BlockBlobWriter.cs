using System;
using System.IO;

namespace TestPossessed.Azure.Storage
{
    public class BlockBlobWriter : IBlockBlobWriter
    {
        private readonly IBlobContainer blobContainer;
        private readonly ILogWriter logWriter;
        private readonly IMetricFactory metricFactory;

        public BlockBlobWriter(ILogWriter logWriter, IMetricFactory metricFactory, IBlobContainer blobContainer)
        {
            this.logWriter = logWriter;
            this.metricFactory = metricFactory;
            this.blobContainer = blobContainer;
        }

        public Uri Write(Stream stream, string key)
        {
            using(this.metricFactory.CreateLoggingTimerMetric(this.logWriter)
                      .Start("BlockBlobWriter.Write"))
            {
                this.logWriter.Trace($"Adding blob to container with key {key}");
                var blob = this.blobContainer.GetBlockBlob(key);
                blob.UploadFromStream(stream);
                return blob.Uri;
            }
        }

        public Uri Write(string text, string key)
        {
            using (this.metricFactory.CreateLoggingTimerMetric(this.logWriter)
                      .Start("BlockBlobWriter.Write"))
            {
                this.logWriter.Trace($"Adding blob to container with key {key}");
                var blob = this.blobContainer.GetBlockBlob(key);
                blob.UploadText(text);
                return blob.Uri;
            }
        }
    }
}