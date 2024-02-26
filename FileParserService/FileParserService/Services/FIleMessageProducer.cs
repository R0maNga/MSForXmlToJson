using System.Text;
using FileParserService.Services.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace FileParserService.Services
{
    public class FIleMessageProducer : IFileMessageProducer
    {
        string rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
        public void SendMessage<T>(T message, string queueName)
        {

            var factory = new ConnectionFactory { HostName = rabbitMqHost };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body);

        }
    }
}
