namespace TestPossessed.Azure.Storage
{
    public class QueueWriter : IQueueWriter
    {
        private readonly ILogWriter logWriter;
        private readonly IMetricFactory metricFactory;
        private readonly IStorageQueue storageQueue;

        public QueueWriter(ILogWriter logWriter, IMetricFactory metricFactory, IStorageQueue storageQueue)
        {
            this.logWriter = logWriter;
            this.metricFactory = metricFactory;
            this.storageQueue = storageQueue;
        }

        public void Write(IQueueMessage queueMessage)
        {
            using(this.metricFactory.CreateLoggingTimerMetric(this.logWriter)
                      .Start("QueueWriter.Write"))
            {
                this.logWriter.Trace($"Adding message to queue with content {queueMessage.AsString()}");
                this.storageQueue.AddMessage(queueMessage);
            }
        }

        public void Write(string content)
        {
            this.Write(new QueueMessage(content));
        }
    }
}