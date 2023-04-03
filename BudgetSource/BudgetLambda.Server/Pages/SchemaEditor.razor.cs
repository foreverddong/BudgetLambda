using System.Security.Claims;
using BudgetLambda.CoreLib.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        private bool creation = false;


        private List<DataSchema> schemas { get; set; }
        private DataSchema selectedSchema { get; set; }
        private DataSchema newSchema { get; set; }


        //protected override async Task OnInitializedAsync()
        //{
        //    this.schemas = await this.ObtainSchemas();
        //}

        //private async Task<List<DataSchema>> ObtainSchemas()
        //{
        //    var components = database.PipelinePackages.Where(p => p.PackageID == packageid).SelectMany(p => p.ChildComponents);
        //    var schemas = components.Select(c => c.InputSchema).UnionBy(components.Select(c => c.OutputSchema), c => c.SchemaID);
        //    return await schemas.ToListAsync();
        //}

        private DataSchema schema = new DataSchema();

        private PropertyDefinitionContext context = new PropertyDefinitionContext();

        private class PropertyDefinitionContext
        {
            public Guid DefinitionID { get; set; } = Guid.NewGuid();
            public DataType Type { get; set; } = DataType.Integer;
            public string Identifier { get; set; } = string.Empty;
        }

        private void AddDefinition()
        {
            schema.Mapping.Add(new PropertyDefinition
            {
                DefinitionID = context.DefinitionID,
                Type = context.Type,
                Identifier = context.Identifier
            });

            context.DefinitionID = Guid.NewGuid();
            context.Type = DataType.Integer;
            context.Identifier = string.Empty;
        }

        private void RemoveDefinition(Guid definitionID)
        {
            schema.Mapping.RemoveAll(x => x.DefinitionID == definitionID);
        }

        private void SaveSchema()
        {
            // Save the schema
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
        //private void SaveSchema()
        //{
        //    // TODO: Save the schema to your data store
        //    if (creation)
        //    {
        //        database.DataSchemas.Add(this.package);
        //    }
        //    await database.SaveChangesAsync();
        //    navigation.NavigateTo($"/packageeditor/{this.package.PackageID}/", true);
        //}
    }

    

}
