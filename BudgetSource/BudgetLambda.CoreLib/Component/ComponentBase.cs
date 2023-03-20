using BudgetLambda.CoreLib.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Component
{
    public abstract class ComponentBase
    {
        public Guid ComponentID { get; set; } = Guid.NewGuid();
        public string ComponentName { get; set; }
        public DataSchema InputSchema { get; set; }
        public DataSchema OutputSchema { get; set; }
        public List<ComponentBase> Next { get; set; }

    }
}
