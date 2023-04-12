using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Component
{
    /// <summary>
    /// An enum Representing programming languages support by the BudgetLambda platform. 
    /// </summary>
    public enum Language
    {
        /// <summary>
        /// C#
        /// </summary>
        CSHARP,
        /// <summary>
        /// JavaScript
        /// </summary>
        JAVASCRIPT,
        /// <summary>
        /// Not related to any programming language, e.g. a source or a built-in sink.
        /// </summary>
        NONE,
    }
}
