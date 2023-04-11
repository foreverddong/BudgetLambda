using BudgetLambda.CoreLib.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BudgetLambda.Server.Pages
{
    public partial class SchemaEditor
    {
        [Parameter]
        public PipelinePackage Package { get; set; }

    }
}
