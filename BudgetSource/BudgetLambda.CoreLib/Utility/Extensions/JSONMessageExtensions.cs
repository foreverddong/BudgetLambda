using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Utility.Extensions
{
    /// <summary>
    /// Helper extension to dump textual message from a JSON Message.
    /// </summary>
    public static class JSONMessageExtensions
    {
        /// <summary>
        /// Dumps the stream into string format.
        /// </summary>
        /// <param name="m">
        /// the JSONMessage object to retrive message from.
        /// </param>
        public static void DumpStream(this JSONMessage m)
        {
            Console.WriteLine(m.Stream);
        }
    }
}
