
using BudgetLambda.CoreLib.Utility.Extensions;
using BudgetLambda.CoreLib.Utility.Faas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Component
{
    public class PipelinePackage
    {
        [Key]
        public Guid PackageID { get; set; } = Guid.NewGuid();
        public string Tenant { get; set; }
        public string PackageName { get; set; }
        public virtual ComponentBase? Source { get; set; }
        //should be a list of schemas
        public virtual List<DataSchema>? Schamas { get; set; } = new();

        public virtual List<ComponentBase>? ChildComponents { get; set; } = new();

        public string ExchangeName => $"ex-{PackageID.ShortID()}-{PackageName}";

        public bool Validate()
        {
//#warning TODO
            /*
             *   Things to consider here:
             *   1. There must be no loop in the pipeline
             *   2. Input/Output schema must match between components
             *   3. Sinks must be without any output schema
             */

            // return true for now.
            return true;
        }

        public async Task<List<(ComponentBase me, bool status, string message)>> CheckHealth(FaasClient client)
        {
            if (this.Source is null)
            {
                return new();
            }
            var healthTasks = this.Source.AllChildComponents().Select(c => c.HealthCheck(client)).ToList();
            var result = (await Task.WhenAll(healthTasks)).ToList();
            return result;
            
        }
        public void ConfigurePackage()
        {
            var starting = this.Source;
            starting.ConfigureKey("");
        }

        public async Task PurgePipeline(FaasClient client)
        {
            var deletionTasks = this.ChildComponents.Select(c => 
            {
                return client.FunctionsDELETEAsync(new DeleteFunctionRequest 
                {
                    FunctionName = c.ServiceName,
                });
            });
            try
            {
                await Task.WhenAll(deletionTasks);
            }
            catch (Exception) { }
        }

        public List<ComponentBase> FindOrphanedComponents() => this.ChildComponents.Except(this.Source.AllChildComponents()).ToList();
    }
}
