
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
    /// <summary>
    /// Represening a complete pipeline package, containing its schema definitions, child components as well as metadata.
    /// </summary>
    public class PipelinePackage
    {
        /// <summary>
        /// The unique id associated with this object, used in the database as a Primary Key.
        /// </summary>
        [Key]
        public Guid PackageID { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Email of the user who owns this package.
        /// </summary>
        public string Tenant { get; set; }
        /// <summary>
        /// The name of the package.
        /// </summary>
        public string PackageName { get; set; }
        /// <summary>
        /// The source component of this package, there could only be one source and data is received from this source before passing
        /// onto the rest of the pipeline.
        /// </summary>
        public virtual ComponentBase? Source { get; set; }
        /// <summary>
        /// A list of data schemas defined in this package to be used by its child components.
        /// </summary>
        public virtual List<DataSchema>? Schamas { get; set; } = new();
        /// <summary>
        /// A collection of all child components in this package.
        /// </summary>
        public virtual List<ComponentBase>? ChildComponents { get; set; } = new();

        /// <summary>
        /// The name of the master exchange for this package. All child components send data to this exchange as well as receiving data from this exchange.
        /// </summary>
        public string ExchangeName => $"ex-{PackageID.ShortID()}-{PackageName}";

        /// <summary>
        /// TODO.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Checks the health status of all child components.
        /// </summary>
        /// <param name="client">
        /// An OpenFaas swagger client to be used.
        /// </param>
        /// <returns>
        /// A 3-tuple consisting of each child component, whether is's healther and if not health, its error message.
        /// </returns>
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
        /// <summary>
        /// Configures the package as well as its child components for their input and output keys.
        /// </summary>
        public void ConfigurePackage()
        {
            var starting = this.Source;
            starting.ConfigureKey("");
        }

        /// <summary>
        /// Purges the pipeline, removing all deployed child components and essentially bring the whole package offline.
        /// </summary>
        /// <param name="client">
        /// An OpenFaas swagger client to be used.
        /// </param>
        /// <returns></returns>
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

        /// <summary>
        /// Finds all orphaned components which are not reachable from the source component.
        /// </summary>
        /// <returns>
        /// Child components not reachable from the source component.
        /// </returns>
        public List<ComponentBase> FindOrphanedComponents() => this.ChildComponents.Except(this.Source.AllChildComponents()).ToList();
    }
}
