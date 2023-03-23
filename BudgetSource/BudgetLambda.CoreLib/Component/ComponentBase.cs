using BudgetLambda.CoreLib.Business;
using BudgetLambda.CoreLib.Utility.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetLambda.CoreLib.Utility.Faas;

namespace BudgetLambda.CoreLib.Component
{
    public abstract class ComponentBase
    {
        [Key]
        public Guid ComponentID { get; set; } = Guid.NewGuid();
        public string ComponentName { get; set; }
        public DataSchema InputSchema { get; set; }
        public DataSchema OutputSchema { get; set; }
        public string InputKey { get; set; }
        public string OutputKey { get; set; }
        public List<ComponentBase> Next { get; set; }

        public virtual string ImageTag => $"registry.donglinxu.com/budgetuser/{this.ComponentID.ShortID()}-{this.ComponentName}:latest";

        public abstract Task<MemoryStream> CreateWorkingPackage(string workdir, string packagedir, IConfiguration configuration);
        public abstract Task<bool> BuildImage(MemoryStream tarball, IConfiguration configuration);
        public abstract FunctionDefinition GenerateDeploymentManifest( string masterExchange, IConfiguration configuration);


    }
}
