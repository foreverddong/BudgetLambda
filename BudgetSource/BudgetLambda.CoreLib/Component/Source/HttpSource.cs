﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using BudgetLambda.CoreLib.Utility.Extensions;
using BudgetLambda.CoreLib.Utility.Faas;
using BudgetLambda.CoreLib.Component.Interfaces;

namespace BudgetLambda.CoreLib.Component.Source
{
    /// <summary>
    /// Represents a source that receives messages from an HTTP endpoint.
    /// </summary>
    [BudgetComponent(ComponentType.Source, "Source - Http")]
    public class HttpSource : ComponentBase, ISource
    {
        /// <inheritdoc />
        public override string ImageTag => "registry-ui.donglinxu.com/budget/httpsource:latest";

        /// <inheritdoc />
        public override ComponentType Type => ComponentType.Source;
        /// <inheritdoc />
        public override string? ServiceName => $"source-{this.ComponentID.ShortID()}-{this.ComponentName}".ToLower();
        /// <summary>
        /// The service uri that receives message in relation to the base address to the Faas Gateway.
        /// </summary>
        public string ServiceUri => $"/function/{ServiceName}/msg";

        /// <inheritdoc />
        public override Task<MemoryStream> CreateWorkingPackage(string workdir, IConfiguration configuration)
        {
            return Task.FromResult((MemoryStream)null);
        }
        /// <inheritdoc />
        public override Task<bool> BuildImage(MemoryStream tarball, IConfiguration configuration)
        {
            return Task.FromResult(true);
        }
        /// <inheritdoc />
        public override FunctionDefinition GenerateDeploymentManifest(string masterExchange, IConfiguration configuration)
        {
            var res = new FunctionDefinition
            {
                Service = ServiceName,
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
