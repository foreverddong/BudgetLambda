﻿using BudgetLambda.CoreLib.Component;
using BudgetLambda.CoreLib.Utility.Extensions;
using BudgetLambda.CoreLib.Utility.Faas;
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
        private readonly FaasClient client;
        private PipelinePackage package;


        public BudgetWorkloadScheduler(IConfiguration conf, FaasClient _client)
        {
            this.configuration = conf;
            this.client = _client;
        }

        public void LoadPackage(PipelinePackage package)
        {
            this.package = package;
        }

        public async Task ConfigureMQ()
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration.GetValue<string>("Infrastructure:RabbitMQ:Hostname"),
                UserName = configuration.GetValue<string>("Infrastructure:RabbitMQ:Username"),
                Password = configuration.GetValue<string>("Infrastructure:RabbitMQ:Password"),
                VirtualHost = configuration.GetValue<string>("Infrastructure:RabbitMQ:VirtualHost"),
            };

            await Task.Run(() => 
            {
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.ExchangeDeclare(this.package.ExchangeName, ExchangeType.Topic, durable: true);
            });
        }

        public async Task SchedulePackage(string workdir, Action<int> increaseCount)
        {
            package.ConfigurePackage();
            var allcomponents = package.Source.AllChildComponents();
            int increment = 100 / allcomponents.Count;
            foreach (var c in allcomponents) 
            {
                await this.ScheduleComponent(workdir, c);
                increaseCount(increment);
            }
        }

        public async Task ScheduleComponent(string workdir, ComponentBase component)
        {
            if (!Path.Exists(workdir))
            {
                Directory.CreateDirectory(workdir);
            }
            var package = await component.CreateWorkingPackage(workdir, configuration);
            await component.BuildImage(package, configuration);
            var manifest = component.GenerateDeploymentManifest(this.package.ExchangeName, configuration);
            await client.FunctionsPOSTAsync(manifest);
            Directory.Delete(workdir, true);

        }
    }
}
