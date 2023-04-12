using BudgetLambda.CoreLib.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BudgetLambda.Server.Pages
{
    public partial class SchemaEditor
    {
        [Parameter]
        public PipelinePackage Package { get; set; }

        public DataSchema? SelectedSchema { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        public async Task CreateSchema()
        {
            var schema = new DataSchema() { SchemaName = "New Schema", };
            this.Package.Schamas.Add(schema);
            this.database.Add(schema);
            await database.SaveChangesAsync();
        }

        public async Task DeleteSchema(DataSchema s)
        {
            this.database.Remove(s);
            await database.SaveChangesAsync();
        }

        public async Task AddDefinition()
        {
            var def = new PropertyDefinition { Type = DataType.String, Identifier = "New Variable" };
            database.Add(def);
            this.SelectedSchema.Mapping.Add(def);
            await database.SaveChangesAsync();
        }

        public async Task DeleteDefinition(PropertyDefinition s)
        {
            database.Remove(s);
            this.SelectedSchema.Mapping.Remove(s);
            await database.SaveChangesAsync();
        }
    }
}
