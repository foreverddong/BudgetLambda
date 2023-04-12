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
    [BudgetComponent(ComponentType.Map,"Lambda Map - C#", Language.CSHARP)]
    public class CSharpLambdaMap : ComponentBase, ILambdaMap
    {
        public Language Lang => Language.CSHARP;

        public string Code { get; set; }

        public override ComponentType Type => ComponentType.Map;
        public override string? ServiceName => $"func-{this.ComponentID.ShortID()}-{this.ComponentName}".ToLower();

        public override async Task<MemoryStream> CreateWorkingPackage(string workdir, IConfiguration configuration)
        {
            //Download the dockerfile
            var dockerfileUri = configuration.GetValue<string>("Components:CSharpLambdaMap:DockerfileUri");
            var client = new HttpClient();
            var response = await client.GetAsync(dockerfileUri);
            using (var fs = new FileStream($"{workdir}/Dockerfile", FileMode.CreateNew))
            {
                await response.Content.CopyToAsync(fs);
            }
            //Write the models and the handler
            using (var fs = new FileStream($"{workdir}/InputModel.cs", FileMode.CreateNew))
            using (var writer = new StreamWriter(fs))
            {
                await writer.WriteLineAsync(this.ScaffoldInputModel());
            }

            using (var fs = new FileStream($"{workdir}/OutputModel.cs", FileMode.CreateNew))
            using (var writer = new StreamWriter(fs))
            {
                await writer.WriteLineAsync(this.ScaffoldOutputModel());
            }

            using (var fs = new FileStream($"{workdir}/Handler.cs", FileMode.CreateNew))
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

        private string ScaffoldInputModel()
        {
            // I understand that this is quite ugly, let's improve this later...
            string prefix =
"""
namespace CSharpFunction
{
    public class InputModel
    {
""";
            string postfix =
"""
    }
}
""";
            var builder = new StringBuilder();
            builder.AppendLine(prefix);
            var decl = this.InputSchema.Mapping.Select(s => $"public {ConvertNativeType(s.Type)} {s.Identifier} {{get; set;}}").Aggregate((a,b) => $"{a}\n{b}");
            builder.AppendLine(decl);
            builder.AppendLine(postfix);
            return builder.ToString();
        }

        private string ScaffoldOutputModel() 
        {
            string prefix =
"""
namespace CSharpFunction
{
    public class OutputModel
    {
""";
            string postfix =
"""
    }
}
""";
            var builder = new StringBuilder();
            builder.AppendLine(prefix);
            var decl = this.OutputSchema.Mapping.Select(s => $"public {ConvertNativeType(s)} {s.Identifier} {{get; set;}}").Aggregate((a, b) => $"{a}\n{b}");
            builder.AppendLine(decl);
            builder.AppendLine(postfix);
            return builder.ToString();
        }

        private string ScaffoldCustomFunction()
        {
            string prefix =
"""
namespace CSharpFunction
{
    public partial class Handler
    {
""";
            string postfix =
"""
    }
}
""";
            var builder = new StringBuilder();
            builder.AppendLine(prefix);
            builder.AppendLine(this.Code);
            builder.AppendLine(postfix);
            return builder.ToString();
        }

        private string ConvertNativeType(PropertyDefinition d)
        {
            var t = d.Type;
            var typename =  t switch
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
