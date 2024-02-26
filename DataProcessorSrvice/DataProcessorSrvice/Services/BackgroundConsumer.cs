using System.Text;
using DataProcessorService.DbContext;
using DataProcessorService.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using DataProcessorService.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DataProcessorService.Services
{
    public class BackgroundConsumerForJson : BackgroundService
    {
        readonly IFinder finder = new Finder();



        private readonly DataProcessorDbContext _context;

        public BackgroundConsumerForJson(DataProcessorDbContext context)
        {
            _context = context;
        }
        
        protected override async Task ExecuteAsync(CancellationToken token)
        {
            string rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost"; 
            string rabbitMqQueueName = Environment.GetEnvironmentVariable("RABBITMQ_QUEUE") ?? "xmlToJsonQueue";
            var factory = new ConnectionFactory() { HostName = rabbitMqHost };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            {
                channel.QueueDeclare(queue: rabbitMqQueueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var moduleState = finder.FindAllData(message, "ModuleState");
                    var moduleCategoryId = finder.FindAllModuleCategoryIds(message);
                    await AddOrUpdateRange(moduleCategoryId, moduleState);
                    
                   
                };

                channel.BasicConsume(queue: rabbitMqQueueName,
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine("Waiting for messages. Press [Ctrl+C] to exit.");
                
                await Task.Delay(-1, token);

            }
        }



        public async Task AddOrUpdateRange(List<string> moduleCategoryIds, List<string> moduleStates)
        {
            if (moduleCategoryIds.Count != moduleStates.Count)
            {
                throw new ArgumentException("The number of moduleCategoryIds must be equal to the number of moduleStates.");
            }

            foreach (var moduleId in moduleCategoryIds)
            {
                var existingCategory = await _context.RapidControlStatus.FirstOrDefaultAsync(r => r.ModuleCategoryID == moduleId);

                if (existingCategory != null)
                {
                    var index = moduleCategoryIds.IndexOf(moduleId);
                    existingCategory.ModuleState = moduleStates[index];
                    _context.RapidControlStatus.Update(existingCategory);
                }
                else
                {
                    var index = moduleCategoryIds.IndexOf(moduleId);
                    var moduleCategory = new RapidControlStatus()
                    {
                        ModuleCategoryID = moduleId,
                        ModuleState = moduleStates[index]
                    };
                    _context.RapidControlStatus.Add(moduleCategory);
                }
            }

            await _context.SaveChangesAsync();
        }




    }
}

