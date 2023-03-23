using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Utility.Extensions
{
    public static class GuidExtensions
    {
        public static string ShortID(this Guid self)
        {
            return self.ToString()[0..8];
        }
    }
}
