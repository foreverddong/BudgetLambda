using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Utility.Extensions
{
    /// <summary>
    /// Static Extension class for Guid type.
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// Obtain a short id used to simplify Guids.
        /// </summary>
        /// <param name="self">
        /// the Guid to be simplified.
        /// </param>
        /// <returns>
        /// the first 8 characters of this guid.
        /// </returns>
        public static string ShortID(this Guid self)
        {
            return self.ToString()[0..8];
        }
    }
}
