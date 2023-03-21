using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BakedStdoutSink
{
    public class Program
    {
        public IServiceProvider Services { get; set; }

        static void Main(string[] args)
        {

            new Program().MainAsync(args, ConfigureServices()).GetAwaiter().GetResult();
        }

        public static IServiceProvider ConfigureServices()
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var provider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddLogging(c => c.AddJsonConsole());

            return provider.BuildServiceProvider();
        }

        public async Task MainAsync(string[] args, IServiceProvider _services)
        {
            this.Services = _services;
            this.ConfigureSinkStdOut();
            
            await Task.Delay(Timeout.Infinite);

        }

        private void ConfigureSinkStdOut()
        {
            var configuration = Services.GetRequiredService<IConfiguration>();
            var logger = Services.GetRequiredService<ILogger<Program>>();
            var factory = new ConnectionFactory
            {
                HostName = configuration.GetValue<string>("RabbitMQ:Hostname"),
                UserName = configuration.GetValue<string>("RabbitMQ:Username"),
                Password = configuration.GetValue<string>("RabbitMQ:Password"),
                VirtualHost = configuration.GetValue<string>("RabbitMQ:VirtualHost"),
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var queueName = configuration.GetValue<string>("Pipeline:Queue");
            var exchangeName = configuration.GetValue<string>("Pipeline:Exchange");
            var routingKey = configuration.GetValue<string>("Pipeline:Key");
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queueName, exchangeName, routingKey);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                logger.LogInformation($"{body}");
                channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume(queueName, true, consumer);
        }
    }
}