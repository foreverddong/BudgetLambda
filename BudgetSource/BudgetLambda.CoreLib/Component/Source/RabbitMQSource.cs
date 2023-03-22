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

        public override Task<bool> BuildImage()
        {
            throw new NotImplementedException();
        }

        public override Task<bool> CreateWorkingPackage(string workdir)
        {
            throw new NotImplementedException();
        }

        public override string GenerateDeploymentManifest()
        {
            throw new NotImplementedException();
        }
    }
}
