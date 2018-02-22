namespace TestPossessed.Azure.Storage
{
    public interface IQueueWriter
    {
        void Write(IQueueMessage queueMessage);
        void Write(string content);
    }
}