using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using CSharpFunction;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();


var configuration = app.Configuration;
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
var inputKey = configuration.GetValue<string>("Pipeline:InputKey");
var outputKey = configuration.GetValue<string>("Pipeline:OutputKey");
channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
channel.QueueBind(queueName, exchangeName, inputKey);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (ch, ea) =>
{
    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
    var inputObject = JsonSerializer.Deserialize<InputModel>(body);
    var handler = new Handler();
    var outputObject = handler.HandleData(inputObject);
    var outputBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize<OutputModel>(outputObject));
    channel.BasicPublish(exchange: exchangeName, routingKey: outputKey, basicProperties: null, body: outputBody);
};
channel.BasicConsume(queueName, true, consumer);


app.Run();
