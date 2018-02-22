using TestPossessed.Azure.Storage.Adapters;
using Xunit;

namespace TestPossessed.Azure.Storage.IntegrationTests
{
    public class StorageTests
    {
        [Fact]
        public void CanWriteAndReadBackMessagesOnQueue()
        {
            var connection =
                new StorageConnection(
                    new ConfigFileConnectionStringProvider(),
                    new ConfigFileAppSettingProvider(),
                    new AccountProvider()).Open("PcgStorageAccount");
            var writer = connection.CreateQueueWriter("testing");
            writer.Write(new QueueMessage("message 1"));
            writer.Write(new QueueMessage("message 2"));
            writer.Write(new QueueMessage("message 3"));

            var reader = connection.CreateQueueReader("testing");
            var counter = 0;
            while(reader.Read())
            {
                reader.Current.Should()
                      .Be("message "  + ++counter);
            }
            counter.Should()
                   .Be(3);
        }
    }
}