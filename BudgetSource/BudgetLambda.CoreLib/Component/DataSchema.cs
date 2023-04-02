using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Component
{
    public class DataSchema
    {
        [Key]
        public Guid SchemaID { get; set; } = Guid.NewGuid();
        public string? SchemaName { get; set; }

        public virtual List<PropertyDefinition> Mapping { get; set; } = new();
    }

    public enum DataType
    {
        Integer,
        Float,
        Boolean,
        String,
    }

    public class PropertyDefinition
    {
        [Key]
        public Guid DefinitionID { get; set; } = Guid.NewGuid();
        public DataType Type { get; set; }
        public string Identifier { get; set; }
    }
}
