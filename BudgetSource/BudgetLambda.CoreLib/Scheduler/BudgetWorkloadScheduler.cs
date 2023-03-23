using BudgetLambda.CoreLib.Component;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Scheduler
{
    public class BudgetWorkloadScheduler
    {
        private readonly IConfiguration configuration;
        public BudgetWorkloadScheduler(IConfiguration conf)
        {
            this.configuration = conf;
        }

        public async Task ConfigureMQ(PipelinePackage package)
        {
            var exchangename = $"{package.Tenant.Prefix}-{package.ExchangeName}";
            var factory = new ConnectionFactory
            {
                HostName = configuration.GetValue<string>("RabbitMQ:Hostname"),
                UserName = configuration.GetValue<string>("RabbitMQ:Username"),
                Password = configuration.GetValue<string>("RabbitMQ:Password"),
                VirtualHost = configuration.GetValue<string>("RabbitMQ:VirtualHost"),
            };

            await Task.Run(() => 
            {
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.ExchangeDeclare(exchangename, ExchangeType.Topic, durable: true);
            });
        }

        public async Task SchedulePackage()
        {
        
        }

        public async Task ScheduleComponent(ComponentBase component)
        {
            
        }
    }
}
