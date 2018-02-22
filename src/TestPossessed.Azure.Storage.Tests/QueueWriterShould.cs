namespace TestPossessed.Azure.Storage.Tests
{
    [TestFixture]
    public class QueueWriterShould : MetricConsumerTestBase<IQueueWriter>
    {
        [Fact]
        public void UseFactoryToCreateMetric()
        {
            this.AssumeWriteWasRequested();
            this.AssertFactoryWasUsedToCreateMetric();
        }

        [Fact]
        public void StartMetric()
        {
            this.AssumeWriteWasRequested();
            this.AssertMetricWasStartedWith("Write");
        }

        [Fact]
        public void UseQueueToWriteMessage()
        {
            this.AssumeWriteWasRequested();
            this.storageQueue.Received()
                .AddMessage(this.queueMessage);
        }

        [Fact]
        public void UseQueueToWriteContent()
        {
            const string Content = "content";
            this.target.Write(Content);
            this.storageQueue.Received()
                .AddMessage(Arg.Is<QueueMessage>(m => m.AsString() == Content));
        }

        private IQueueMessage queueMessage;
        private IStorageQueue storageQueue;

        protected override IQueueWriter CreateTestTarget(ILogWriter logWriter, IMetricFactory metricFactory)
        {
            this.AssumeQueueIsInitialised();
            return new QueueWriter(logWriter, metricFactory, this.storageQueue);
        }

        private void AssumeQueueIsInitialised()
        {
            this.storageQueue = Substitute.For<IStorageQueue>();
            this.queueMessage = Substitute.For<IQueueMessage>();
        }

        private void AssumeWriteWasRequested()
        {
            this.target.Write(this.queueMessage);
        }
    }
}