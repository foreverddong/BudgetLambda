using BudgetLambda.CoreLib.Utility.Faas;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Component.Source
{
    public class RabbitMQSource : ComponentBase
    {
        public string ExchangeName { get; set; }

        public override ComponentType Type => throw new NotImplementedException();

        public override Task<bool> BuildImage(MemoryStream tarball, IConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public override Task<MemoryStream> CreateWorkingPackage(string workdir, IConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public override FunctionDefinition GenerateDeploymentManifest(string masterExchange, IConfiguration configuration)
        {
            throw new NotImplementedException();
        }
    }
}
