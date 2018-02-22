using Microsoft.WindowsAzure.Storage.Queue;

namespace TestPossessed.Azure.Storage.Adapters
{
    public class QueueMessage : IQueueMessage
    {
        internal QueueMessage(CloudQueueMessage cloudQueueMessage)
        {
            this.CloudQueueMessage = cloudQueueMessage;
        }

        public QueueMessage(string content)
        {
            this.CloudQueueMessage = new CloudQueueMessage(content);
        }

        internal CloudQueueMessage CloudQueueMessage { get; set; }

        public string AsString()
        {
            return this.CloudQueueMessage.AsString;
        }
    }
}