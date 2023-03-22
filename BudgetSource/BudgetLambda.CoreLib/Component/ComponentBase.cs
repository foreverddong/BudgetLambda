using BudgetLambda.CoreLib.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public abstract Task<bool> CreateWorkingPackage(string workdir);
        public abstract Task<bool> BuildImage();
        public abstract string GenerateDeploymentManifest();


    }
}
