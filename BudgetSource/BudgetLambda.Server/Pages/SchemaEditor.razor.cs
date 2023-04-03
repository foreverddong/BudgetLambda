using System.Security.Claims;
using BudgetLambda.CoreLib.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BudgetLambda.Server.Pages
{
    public partial class SchemaEditor
    {
        [Parameter]
        public Guid packageid { get; set; }
        private Task<AuthenticationState>? authenticationState { get; set; }
        private ClaimsPrincipal User { get; set; }
        private PipelinePackage? package;
        private bool healthy = false;

        private List<DataSchema> schemas { get; set; }
        private DataSchema selectedSchema { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.schemas = await this.ObtainSchemas();
        }

        private async Task<List<DataSchema>> ObtainSchemas()
        {
            var components = database.PipelinePackages.Where(p => p.PackageID == packageid).SelectMany(p => p.ChildComponents);
            var schemas = components.Select(c => c.InputSchema).UnionBy(components.Select(c => c.OutputSchema), c => c.SchemaID);
            return await schemas.ToListAsync();
        }

        //protected override async Task OnInitializedAsync()
        //{
        //    this.User = (await authenticationState).User;           
            
        //    var res = (from p in database.PipelinePackages
        //                where p.PackageID == packageid
        //                select p).ToList();
        //    this.package = res.First();

        //    this.healthy = await package.CheckHealth();

        //}


        // save schema
        private void SaveSchema()
        {
            // TODO: Save the schema to your data store
        }


    }

}
