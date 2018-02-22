using System;

namespace TestPossessed.Azure.Storage
{
    public sealed class QueueReader : IQueueReader
    {
        private const string MessageVisibilityTimeoutMinutesKey = "MessageVisibilityTimeoutMinutes";
        private readonly TimeSpan defaultVisibilityTimeout;
        private readonly ILogWriter logWriter;
        private readonly IMetricFactory metricFactory;
        private readonly IStorageQueue storageQueue;
        private IQueueMessage currentMessage;

        public QueueReader(ILogWriter logWriter,
            IMetricFactory metricFactory,
            IAppSettingProvider appSettingProvider,
            IStorageQueue storageQueue)
        {
            this.logWriter = logWriter;
            this.metricFactory = metricFactory;
            this.storageQueue = storageQueue;
            this.defaultVisibilityTimeout = appSettingProvider.GetTimespan(MessageVisibilityTimeoutMinutesKey);
        }

        public string Current { get; private set; }

        public bool Read(bool deleteMessage = true)
        {
            return this.Read(this.defaultVisibilityTimeout, deleteMessage);
        }

        public bool Read(TimeSpan visibilityTimeOut, bool deleteMessage = true)
        {
            using(this.metricFactory.CreateLoggingTimerMetric(this.logWriter)
                      .Start("QueueReader.Read"))
            {
                if(this.currentMessage != null && deleteMessage)
                {
                    this.logWriter.Trace("Deleting previous message from queue");
                    this.storageQueue.DeleteMessage(this.currentMessage);
                }

                this.logWriter.Trace("Checking for next message");
                this.currentMessage = this.storageQueue.GetMessage(visibilityTimeOut);
                if(this.currentMessage == null)
                {
                    this.logWriter.Trace("No more messages on queue");
                    this.Current = null;
                    return false;
                }

                this.Current = this.currentMessage.AsString();
                return true;
            }
        }
    }
}