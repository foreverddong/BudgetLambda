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

namespace BudgetLambda.CoreLib.Component.Map
{
    public class CSharpLambdaMap : ComponentBase
    {
        public Language Lang => Language.CSHARP;

        public string Code { get; set; }

        public override async Task<MemoryStream> CreateWorkingPackage(string workdir, string packagedir, IConfiguration configuration)
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
            var tarName = $"{packagedir}/{this.ComponentID.ShortID()}-{this.ComponentName}.tar";
            MemoryStream ms = new MemoryStream();
            await TarFile.CreateFromDirectoryAsync(workdir,
                ms,
                includeBaseDirectory: false);
            return ms;



        }
        public override async Task<bool> BuildImage(MemoryStream tarball, IConfiguration configuration)
        {
            var client = new DockerClientConfiguration(new Uri(configuration.GetValue<string>("Infrastructure:Docker:ServerUri"))).CreateClient();
            var parameters = new ImageBuildParameters
            {
                Tags = new List<string> { this.ImageTag }
            };
            await client.Images.BuildImageFromDockerfileAsync( parameters, tarball, null, null, null);
            await client.Images.PushImageAsync(this.ImageTag, null, null, null);

            return true;
        }
        public override string GenerateDeploymentManifest(string masterExchange)
        {
            throw new NotImplementedException();
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
            var decl = this.InputSchema.Mapping.Select(s => $"public {s.Type} {s.Identifier} {{get; set;}}").Aggregate((a,b) => $"{a}\n{b}");
            builder.AppendLine(decl);
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
            var decl = this.OutputSchema.Mapping.Select(s => $"public {s.Type} {s.Identifier} {{get; set;}}").Aggregate((a, b) => $"{a}\n{b}");
            builder.AppendLine(decl);
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
    }
}
