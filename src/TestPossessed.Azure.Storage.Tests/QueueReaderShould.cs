using System;

namespace TestPossessed.Azure.Storage.Tests
{
    [TestFixture]
    public class QueueReaderShould : MetricConsumerTestBase<IQueueReader>
    {
        [Fact]
        public void UseProviderToGetDefaultMessageVisibility()
        {
            this.appSettingProvider.Received()
                .GetTimespan("MessageVisibilityTimeoutMinutes");
        }

        [Fact]
        public void UseFactoryToCreateMetric()
        {
            this.AssumeReadWasRequested();
            this.AssertFactoryWasUsedToCreateMetric();
        }

        [Fact]
        public void StartMetric()
        {
            this.AssumeReadWasRequested();
            this.AssertMetricWasStartedWith("Read");
        }

        [Fact]
        public void UseQueueToGetMessage()
        {
            this.AssumeReadWasRequested();
            this.storageQueue.Received()
                .GetMessage(Arg.Any<TimeSpan>());
        }

        [Fact]
        public void UseDefaultTimeoutIfNotSpecifiedOnRead()
        {
            this.AssumeReadWasRequested();
            this.storageQueue.Received()
                .GetMessage(this.defaultTimeout);
        }

        [Fact]
        public void UseTimeoutIfSpecifiedOnRead()
        {
            var timeOut = TimeSpan.FromMinutes(1);
            this.target.Read(timeOut);
            this.storageQueue.Received()
                .GetMessage(timeOut);
        }

        [Fact]
        public void IndicateMessageWasRetrieved()
        {
            this.AssumeReadWasRequested().Should().BeTrue();
        }

        [Fact]
        public void IndicateWhenNoMessageAvailable()
        {
            this.AssumeNoMessageOnQueue();
            this.AssumeReadWasRequested()
                .Should()
                .BeFalse();
        }

        [Fact]
        public void CaptureMessageContentAsString()
        {
            this.AssumeReadWasRequested();
            this.target.Current.Should()
                .Be(MessageContent);
        }

        [Fact]
        public void ClearCurrrentIfNoMessageAvailable()
        {
            this.AssumeReadWasRequested();
            this.AssumeNoMessageOnQueue();
            this.AssumeReadWasRequested();
            this.target.Current.Should()
                .BeNull();
        }

        [Fact]
        public void DeletePreviousMessageBeforeGettingNext()
        {
            this.AssumeReadWasRequested();
            this.AssumeReadWasRequested();
            this.storageQueue.Received().DeleteMessage(this.queueMessage);
        }

        private void AssumeNoMessageOnQueue()
        {
            this.storageQueue.GetMessage(Arg.Any<TimeSpan>())
                .Returns((IQueueMessage)null);
        }

        private IAppSettingProvider appSettingProvider;
        private readonly TimeSpan defaultTimeout = TimeSpan.FromMinutes(5);
        private IQueueMessage queueMessage;
        private IStorageQueue storageQueue;
        private const string MessageContent = "{message: 'MyMessage'}";

        protected override IQueueReader CreateTestTarget(ILogWriter logWriter, IMetricFactory metricFactory)
        {
            this.AssumeAppSettingProviderIsInitialised();
            this.AssumeQueueIsInitialised();
            return new QueueReader(logWriter, metricFactory, this.appSettingProvider, this.storageQueue);
        }

        private void AssumeAppSettingProviderIsInitialised()
        {
            this.appSettingProvider = Substitute.For<IAppSettingProvider>();
            this.appSettingProvider.GetTimespan("MessageVisibilityTimeoutMinutes")
                .Returns(this.defaultTimeout);
        }

        private void AssumeQueueIsInitialised()
        {
            this.storageQueue = Substitute.For<IStorageQueue>();
            this.queueMessage = Substitute.For<IQueueMessage>();
            this.storageQueue.GetMessage(Arg.Any<TimeSpan>())
                .Returns(this.queueMessage);
            this.queueMessage.AsString()
                .Returns(MessageContent);
        }

        private bool AssumeReadWasRequested()
        {
            return this.target.Read();
        }
    }
}