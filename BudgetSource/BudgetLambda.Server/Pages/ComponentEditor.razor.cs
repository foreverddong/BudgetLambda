using Microsoft.AspNetCore.Components;
using ComponentBase = BudgetLambda.CoreLib.Component.ComponentBase;

namespace BudgetLambda.Server.Pages
{
    public partial class ComponentEditor
    {
        [Parameter]
        public ComponentBase Component { get; set; }

    }
}
