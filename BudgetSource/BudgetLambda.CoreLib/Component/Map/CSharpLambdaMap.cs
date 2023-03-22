using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Component.Map
{
    public class CSharpLambdaMap : ComponentBase
    {
        public Language Lang => Language.CSHARP;

        public string Code { get; set; }

        public override Task<bool> CreateWorkingPackage(string workdir)
        {
            throw new NotImplementedException();
        }
        public override Task<bool> BuildImage()
        {
            throw new NotImplementedException();
        }
        public override string GenerateDeploymentManifest()
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
