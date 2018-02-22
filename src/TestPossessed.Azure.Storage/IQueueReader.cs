using System;

namespace TestPossessed.Azure.Storage
{
    public interface IQueueReader
    {
        string Current { get; }
        bool Read(bool deleteMessage = true);
        bool Read(TimeSpan visibilityTimeOut, bool deleteMessage = true);
    }
}