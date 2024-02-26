namespace FileParserService.Services.Interfaces
{
    public interface IFileMessageProducer
    {
        void SendMessage<T>(T message, string queueName);
    }
}
