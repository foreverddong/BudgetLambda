using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Component
{
    [AttributeUsage(AttributeTargets.Class)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class BudgetComponentAttribute : Attribute

    {
        public ComponentType Type { get; set; }
        public Language Language { get; set; }
        public string Identifier { get; set; }


        public BudgetComponentAttribute(ComponentType type, string identifier, Language language = Language.NONE)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            Type = type;
            Language = language;
            Identifier = identifier;
        }

    }
}
