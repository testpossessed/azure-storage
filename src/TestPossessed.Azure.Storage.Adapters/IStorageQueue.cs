using System;

namespace TestPossessed.Azure.Storage.Adapters
{
    public interface IStorageQueue
    {
        void AddMessage(IQueueMessage message);
        void CreateIfNotExist();
        void DeleteMessage(IQueueMessage message);
        IQueueMessage GetMessage(TimeSpan timeOut);
    }
}