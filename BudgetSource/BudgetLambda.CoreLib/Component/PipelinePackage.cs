
using BudgetLambda.CoreLib.Utility.Extensions;
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
        public virtual List<DataSchema> Schamas { get; set; } = new(); 

        public virtual List<ComponentBase>? ChildComponents { get; set; }

        public string ExchangeName => $"{PackageID.ShortID()}-{PackageName}";

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

        public async Task<bool> CheckHealth()
        {
//#warning TODO
            // Check the health of the entire pipeline.
            return true;
        }
        public void ConfigurePackage()
        {
            var starting = this.Source;
            starting.ConfigureKey("");
        }

        public List<ComponentBase> FindOrphanedComponents()
        {
            return this.ChildComponents.Except(this.Source.AllChildComponents()).ToList();
        }
    }
}
