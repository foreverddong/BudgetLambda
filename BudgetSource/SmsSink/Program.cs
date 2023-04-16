using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Azure.Communication.PhoneNumbers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();
ConfigureSmsSink();

app.Run();

void ConfigureSmsSink()
{
    var configuration = app.Services.GetRequiredService<IConfiguration>();
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
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
    var routingKey = configuration.GetValue<string>("Pipeline:InputKey");
    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
    channel.QueueBind(queueName, exchangeName, routingKey);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (ch, ea) =>
    {
        var body = Encoding.UTF8.GetString(ea.Body.ToArray());
        logger.LogInformation($"{body}");
    };
    channel.BasicConsume(queueName, true, consumer);
}
