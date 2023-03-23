using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using BudgetLambda.CoreLib.Utility.Extensions;

namespace BudgetLambda.CoreLib.Component.Source
{
    public class HttpSource : ComponentBase
    {

        public override Task<MemoryStream> CreateWorkingPackage(string workdir, string packagedir, IConfiguration configuration)
        {
            return Task.FromResult((MemoryStream)null);
        }

        public override async Task<bool> BuildImage(MemoryStream tarball, IConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public override string GenerateDeploymentManifest(string masterExchange)
        {
            throw new NotImplementedException();
        }
    }
}
