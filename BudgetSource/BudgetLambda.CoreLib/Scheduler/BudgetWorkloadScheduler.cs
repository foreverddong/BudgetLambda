using BudgetLambda.CoreLib.Component;
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
    /// <summary>
    /// Scheduler class used to schedule components to the database, this should be injected transiently.
    /// LoadPackage should be called before using the scheduler.
    /// </summary>
    public class BudgetWorkloadScheduler
    {
        private readonly IConfiguration configuration;
        private readonly FaasClient client;
        private PipelinePackage package;

        /// <summary>
        /// Default constructor for dependency injection
        /// </summary>
        /// <param name="conf">
        /// Configuration object, should be injected with at least JSON file and EnvVars support.
        /// </param>
        /// <param name="_client">
        /// OpenFaas swagger client used for interop with the Faas gateway.
        /// </param>
        public BudgetWorkloadScheduler(IConfiguration conf, FaasClient _client)
        {
            this.configuration = conf;
            this.client = _client;
        }

        /// <summary>
        /// Loads a package for scheduling, this method should be called whenever a new instance of the
        /// scheduler is injected.
        /// </summary>
        /// <param name="package">
        /// the package to be used for scheduling
        /// </param>
        public void LoadPackage(PipelinePackage package)
        {
            this.package = package;
        }

        /// <summary>
        /// Configures the RabbitMQ message queue for the use of the package. An exchange will be created based on the exchange key
        /// of the package.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Schedule a package for execution in the Faas platform. Child components for the package
        /// is recursively scheduled.
        /// </summary>
        /// <param name="workdir">
        /// A temp working directory to build the component files in, this directory is transient and will be deleted
        /// after the creation.
        /// </param>
        /// <param name="increaseCount">
        /// an event callback for progress tracking in the web UI.
        /// </param>
        /// <returns></returns>
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

        /// <summary>
        /// Schedules an individual component to run in the cluster. Dependent files will be scaffolded into the
        /// working directory, and a Tarball package will be created from there. The tar package will be then send
        /// to the remote docker server for building and pushing, and then scheduled to run in the cluster.
        /// </summary>
        /// <param name="workdir">
        /// A temp working directory to build the component files in, this directory is transient and will be deleted
        /// after the creation.
        /// </param>
        /// <param name="component">
        /// The component to be scheduled.
        /// </param>
        /// <returns></returns>
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
