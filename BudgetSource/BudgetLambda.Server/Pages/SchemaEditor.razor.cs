using BudgetLambda.CoreLib.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BudgetLambda.Server.Pages
{
    /// <summary>
    /// Web UI page for editing the schemas of a package.
    /// </summary>
    public partial class SchemaEditor
    {
        /// <summary>
        /// The package to read and write data to.
        /// </summary>
        [Parameter]
        public PipelinePackage Package { get; set; }

        private DataSchema? SelectedSchema { get; set; }
/// <inheritdoc/>
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        private async Task CreateSchema()
        {
            var schema = new DataSchema() { SchemaName = "New Schema", };
            this.Package.Schamas.Add(schema);
            this.database.Add(schema);
            await database.SaveChangesAsync();
        }

        private async Task DeleteSchema(DataSchema s)
        {
            this.database.Remove(s);
            await database.SaveChangesAsync();
        }

        private async Task AddDefinition()
        {
            var def = new PropertyDefinition { Type = DataType.String, Identifier = "New Variable" };
            database.Add(def);
            this.SelectedSchema.Mapping.Add(def);
            await database.SaveChangesAsync();
        }

        private async Task DeleteDefinition(PropertyDefinition s)
        {
            database.Remove(s);
            this.SelectedSchema.Mapping.Remove(s);
            await database.SaveChangesAsync();
        }

        private async Task SaveChangesAsync()
        {
            await database.SaveChangesAsync();
        }
    }
}
