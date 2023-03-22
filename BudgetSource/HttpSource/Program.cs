using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
var factory = new ConnectionFactory
{
    HostName = configuration.GetValue<string>("RabbitMQ:Hostname"),
    UserName = configuration.GetValue<string>("RabbitMQ:Username"),
    Password = configuration.GetValue<string>("RabbitMQ:Password"),
    VirtualHost = configuration.GetValue<string>("RabbitMQ:VirtualHost"),
};



var connection = factory.CreateConnection();

builder.Services.AddSingleton<IConnection>(connection);

var app = builder.Build();
app.MapPost("/msg", async (HttpContext context, [FromServices] IConnection conn, [FromServices] IConfiguration conf) => 
{
    using var channel = conn.CreateModel();
    var exchangeName = conf.GetValue<string>("Pipeline:Exchange");
    var routingKey = conf.GetValue<string>("Pipeline:OutputKey");
    var content = await new StreamReader(context.Request.Body, Encoding.UTF8).ReadToEndAsync();
    channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: null,
        body: Encoding.UTF8.GetBytes(content));
});

app.Run();

