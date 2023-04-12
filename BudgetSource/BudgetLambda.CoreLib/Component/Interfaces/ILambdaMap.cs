using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Component.Interfaces
{
    /// <summary>
    /// Denotes the capibility of representing a Lambda Map function component.
    /// </summary>
    public interface ILambdaMap
    {
        /// <summary>
        /// The code for the lambda function.
        /// </summary>
        public string Code { get; set; }
    }
}
