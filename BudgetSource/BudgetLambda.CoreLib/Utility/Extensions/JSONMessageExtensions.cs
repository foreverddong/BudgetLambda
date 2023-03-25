using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Utility.Extensions
{
    public static class JSONMessageExtensions
    {
        public static void DumpStream(this JSONMessage m)
        {
            Console.WriteLine(m.Stream);
        }
    }
}
