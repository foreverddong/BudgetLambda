using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.Json;
using EmailSink;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

var configuration = app.Configuration;
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation(configuration.GetValue<string>("RabbitMQ:Hostname"));
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
channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
channel.QueueBind(queueName, exchangeName, inputKey);

var sender = configuration.GetValue<string>("Email:SenderUsername");
var password = configuration.GetValue<string>("Email:SenderPassword");

//Configure Email
var smtpClient = new SmtpClient
{
    Host = "smtp.office365.com",
    Credentials = new NetworkCredential(sender , password),
    Port = 587,
    DeliveryMethod = SmtpDeliveryMethod.Network,
    EnableSsl = true,
};



var consumer = new EventingBasicConsumer(channel);
consumer.Received += (ch, ea) =>
{
    var msg = JsonSerializer.Deserialize<EmailMsg>(Encoding.UTF8.GetString(ea.Body.ToArray()));
    var mail = new MailMessage
    {
        From = new MailAddress(sender),
        Subject = "BudgetLambda Email Sink Notification",
        Body = msg.Message,
    };
    mail.To.Add(new MailAddress(msg.To));
    smtpClient.Send(mail);
};
channel.BasicConsume(queueName, true, consumer);


app.Run();
