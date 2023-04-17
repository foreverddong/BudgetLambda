using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Formats.Tar;
using BudgetLambda.CoreLib.Utility.Extensions;
using Docker.DotNet;
using Docker.DotNet.Models;
using BudgetLambda.CoreLib.Utility.Faas;
using BudgetLambda.CoreLib.Component.Interfaces;

namespace BudgetLambda.CoreLib.Component.Map
{
    [BudgetComponent(ComponentType.Map, "Lambda Map - JavaScript", Language.JAVASCRIPT)]
    public class JavaScriptLambdaMap : ComponentBase, ILambdaMap
    {
        /// <inheritdoc />
        public Language Lang => Language.JAVASCRIPT;

        /// <inheritdoc />
        public string Code { get; set; } = 
        """
            const handler = (inputObject) => {
                return inputObject
            }
        """;

        /// <inheritdoc />
        public override ComponentType Type => ComponentType.Map;

        /// <inheritdoc />
        public override string? ServiceName => $"func-{this.ComponentID.ShortID()}-{this.ComponentName}".ToLower();

        /// <inheritdoc />
        public override async Task<MemoryStream> CreateWorkingPackage(string workdir, IConfiguration configuration)
        {
            //Download the dockerfile
            var dockerfileUri = configuration.GetValue<string>("Components:JavaScriptLambdaMap:DockerfileUri");
            var client = new HttpClient();
            var response = await client.GetAsync(dockerfileUri);
            using (var fs = new FileStream($"{workdir}/Dockerfile", FileMode.CreateNew))
            {
                await response.Content.CopyToAsync(fs);
            }

            //Write the handler
            using (var fs = new FileStream($"{workdir}/handler.js", FileMode.CreateNew))
            using (var writer = new StreamWriter(fs))
            {
                await writer.WriteLineAsync(this.ScaffoldCustomFunction());
            }

            //Now compress everything into a tarball, thanks to .NET 7
            /*var tarName = $"{Path.GetTempPath()}tartemp/{this.ComponentID.ShortID()}-{this.ComponentName}.tar";
            TarFile.CreateFromDirectory(workdir, tarName, false);*/

            MemoryStream ms = new MemoryStream();
            await TarFile.CreateFromDirectoryAsync(workdir,
                ms,
                includeBaseDirectory: false);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        /// <inheritdoc />
        public override async Task<bool> BuildImage(MemoryStream tarball, IConfiguration configuration)
        {
            var client = new DockerClientConfiguration(new Uri(configuration.GetValue<string>("Infrastructure:Docker:ServerUri"))).CreateClient();
            var parameters = new ImageBuildParameters
            {
                Tags = new List<string> { this.ImageTag }
            };
            await client.Images.BuildImageFromDockerfileAsync(parameters,
                tarball,
                new List<AuthConfig>(),
                new Dictionary<string, string>(),
                new Progress<JSONMessage>(m => m.DumpStream()));
            await client.Images.PushImageAsync(this.ImageTag,
                new ImagePushParameters(),
                new AuthConfig(),
                new Progress<JSONMessage>(s => s.DumpStream()));

            return true;
        }

        /// <inheritdoc />
        public override FunctionDefinition GenerateDeploymentManifest(string masterExchange, IConfiguration configuration)
        {
            var res = new FunctionDefinition
            {
                Service = this.ServiceName,
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
                    { "Pipeline__Queue", $"func-{this.ComponentID.ShortID()}-{this.ComponentName}".ToLower() },
                    { "Pipeline__InputKey", this.InputKey },
                    { "Pipeline__OutputKey" , this.OutputKey},
                },
            };
            return res;
        }

        private string ScaffoldCustomFunction()
        {
            string postfix = 
"""
module.exports = handler
""";
            var builder = new StringBuilder();
            builder.AppendLine(this.Code);
            builder.AppendLine(postfix);
            return builder.ToString();
        }

        private string ConvertNativeType(PropertyDefinition d)
        {
            var t = d.Type;
            var typename = t switch
            {
                DataType.Boolean => "bool",
                DataType.String => "string",
                DataType.Float => "double",
                DataType.Integer => "int",
                _ => throw new NotImplementedException(),
            };
            var listed = $"List<{typename}>";
            return d.IsList ? listed : typename;
        }
    }
}
