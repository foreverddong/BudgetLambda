using BudgetLambda.CoreLib.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BudgetLambda.Server.Pages
{
    public partial class SchemaEditor
    {
        [Parameter]
        public string packageid { get; set; }
        private Guid packageGuid => Guid.Parse(packageid);

        private List<DataSchema> schemas { get; set; }
        private DataSchema selectedSchema { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.schemas = await this.ObtainSchemas();
        }

        private async Task<List<DataSchema>> ObtainSchemas()
        {
            var components = database.PipelinePackages.Where(p => p.PackageID == packageGuid).SelectMany(p => p.ChildComponents);
            var schemas = components.Select(c => c.InputSchema).UnionBy(components.Select(c => c.OutputSchema), c => c.SchemaID);
            return await schemas.ToListAsync();
        }
    }
}
