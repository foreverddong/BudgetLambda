
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
        public virtual DataSchema InputSchema { get; set; }
        public virtual DataSchema OutputSchema { get; set; }
        public string InputKey { get; set; }
        public string OutputKey { get; set; }
        public virtual List<ComponentBase> Next { get; set; } = new();

        public virtual string ImageTag => $"registry.donglinxu.com/budgetuser/{this.ComponentID.ShortID()}-{this.ComponentName.ToLower()}:latest";
        public virtual List<ComponentBase> AllChildComponents => this.Next.SelectMany(c => c.AllChildComponents).Append(this).ToList();

        public abstract ComponentType Type { get; }

        public abstract Task<MemoryStream> CreateWorkingPackage(string workdir, IConfiguration configuration);
        public abstract Task<bool> BuildImage(MemoryStream tarball, IConfiguration configuration);
        public abstract FunctionDefinition GenerateDeploymentManifest( string masterExchange, IConfiguration configuration);
        public virtual void ConfigureKey(string selfInput)
        {
            this.InputKey = selfInput;
            this.OutputKey = $"{this.ComponentID.ShortID()}-{this.ComponentName}-Output";
            foreach (var c in this.Next)
            {
                c.ConfigureKey(this.OutputKey);
            }
        }


    }

    public enum ComponentType
    {
        Source,
        Map,
        Sink,
    }
}
