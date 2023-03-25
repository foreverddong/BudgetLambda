using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using BudgetLambda.CoreLib.Utility.Extensions;
using BudgetLambda.CoreLib.Utility.Faas;

namespace BudgetLambda.CoreLib.Component.Source
{
    public class HttpSource : ComponentBase
    {

        public override string ImageTag => "registry-ui.donglinxu.com/budget/httpsource:latest";

        public override Task<MemoryStream> CreateWorkingPackage(string workdir, IConfiguration configuration)
        {
            return Task.FromResult((MemoryStream)null);
        }

        public override Task<bool> BuildImage(MemoryStream tarball, IConfiguration configuration)
        {
            return Task.FromResult(true);
        }

        public override FunctionDefinition GenerateDeploymentManifest(string masterExchange, IConfiguration configuration)
        {
            var res = new FunctionDefinition
            {
                Service = $"source-{this.ComponentID.ShortID()}-{this.ComponentName}".ToLower(),
                Image = this.ImageTag,
                Network = "deprecated",
                ReadOnlyRootFilesystem = true,
                EnvVars = new Dictionary<string, string>()
                {
                    {"RabbitMQ__Hostname" , configuration.GetValue<string>("Infrastructure:RabbitMQ:Hostname")},
                    {"RabbitMQ__Username" , configuration.GetValue<string>("Infrastructure:RabbitMQ:Username")},
                    {"RabbitMQ__Password" , configuration.GetValue<string>("Infrastructure:RabbitMQ:Password")},
                    { "RabbitMQ__VirtualHost" , "budget"},
                    { "Pipeline__Exchange" , masterExchange},
                    { "Pipeline__Queue", $"source-{this.ComponentID.ShortID()}-{this.ComponentName}".ToLower() },
                    { "Pipeline__OutputKey" , this.OutputKey},
                },
            };
            return res;
        }
    }
}
