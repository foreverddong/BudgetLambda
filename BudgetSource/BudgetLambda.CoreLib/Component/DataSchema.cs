using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Component
{
    /// <summary>
    /// Represents a data schema used by a component.
    /// </summary>
    public class DataSchema
    {
        /// <summary>
        /// The unique key representing this schema, used in database as a Primary Key.
        /// </summary>
        [Key]
        public Guid SchemaID { get; set; } = Guid.NewGuid();
        /// <summary>
        /// The name of the schema
        /// </summary>
        public string? SchemaName { get; set; }
        /// <summary>
        /// A list of variable definitions for this schema.
        /// </summary>

        public virtual List<PropertyDefinition>? Mapping { get; set; } = new();

        /// <inheritdoc />
        public override string ToString() => SchemaName;

    }

    /// <summary>
    /// The variable types enum supported by the project.
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// integer, minimum 32-bit.
        /// </summary>
        Integer,
        /// <summary>
        /// floating-point number, minimum 32-bit.
        /// </summary>
        Float,
        /// <summary>
        /// boolean.
        /// </summary>
        Boolean,
        /// <summary>
        /// string.
        /// </summary>
        String,
    }

    /// <summary>
    /// Represents the definition of a single variable consisting of type, if it's a list and the name of the variable.
    /// </summary>
    public class PropertyDefinition
    {
        /// <summary>
        /// The unique key associated with this definition, used in database as a Primary Key. 
        /// </summary>
        [Key]
        public Guid DefinitionID { get; set; } = Guid.NewGuid();
        
        /// <summary>
        /// If this definition is for a list of variables.
        /// </summary>
        public bool IsList { get; set; } = false;
        /// <summary>
        /// The data type of the variable.
        /// </summary>
        public DataType Type { get; set; }
        /// <summary>
        /// The name of the variable.
        /// </summary>
        public string Identifier { get; set; }
    }
}
