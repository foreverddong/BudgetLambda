using BudgetLambda.CoreLib.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Component
{
    public class PipelinePackage
    {
        public Guid PackageID { get; set; } = Guid.NewGuid();
        public BudgetTenant Tenant { get; set; }
        public string PackageName { get; set; }
        public ComponentBase Source { get; set; }

        public string ExchangeName { get; set; }

        public bool Validate()
        {
#warning TODO
            /*
             *   Things to consider here:
             *   1. There must be no loop in the pipeline
             *   2. Input/Output schema must match between components
             *   3. Sinks must be without any output schema
             */

            // return true for now.
            return true;
        }

        public string GenerateManifest()
        {
            throw new NotImplementedException();
        }
    }
}
