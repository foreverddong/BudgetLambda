
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
        public string? InputKey { get; private set; }
        public string? OutputKey { get; private set; }
        public abstract string? ServiceName { get; }
        public virtual List<ComponentBase> Next { get; set; } = new();

        public virtual string ImageTag => $"registry.donglinxu.com/budgetuser/{this.ComponentID.ShortID()}-{this.ComponentName.ToLower()}:latest";
        public List<ComponentBase> AllChildComponents() 
        { 
            return this.Next.SelectMany(c => c.AllChildComponents()).Append(this).ToList();
        } 

        public abstract ComponentType Type { get; }

        public abstract Task<MemoryStream> CreateWorkingPackage(string workdir, IConfiguration configuration);
        public abstract Task<bool> BuildImage(MemoryStream tarball, IConfiguration configuration);
        public abstract FunctionDefinition GenerateDeploymentManifest(string masterExchange, IConfiguration configuration);
        public virtual void ConfigureKey(string selfInput)
        {
            this.InputKey = selfInput;
            this.OutputKey = $"{this.ComponentID.ShortID()}-{this.ComponentName}-Output";
            foreach (var c in this.Next)
            {
                c.ConfigureKey(this.OutputKey);
            }
        }

        public virtual async Task<(bool status, string message)> HealthCheck(FaasClient client)
        {
            var serviceName = this.ServiceName;
            try
            {
                var res = await client.FunctionGETAsync(serviceName);
                return (true, $"{serviceName} is healthy");
            }
            catch (FaasClientException ex)
            {
                return (false, ex.Message);
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
