using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Component
{
    public class DataSchema
    {
        public Guid SchemaID { get; set; } = Guid.NewGuid();

        public Dictionary<string, string> Mappings { get; set; } = new();
    }

    public enum DataType
    {
        Integer,
        Float,
        Boolean,
        String,
    }
}
