using BudgetLambda.CoreLib.Component;
using BudgetLambda.CoreLib.Component.Interfaces;
using BudgetLambda.CoreLib.Component.Map;
using BudgetLambda.CoreLib.Component.Sink;
using BudgetLambda.CoreLib.Component.Source;
using Microsoft.AspNetCore.Components;
using ComponentBase = BudgetLambda.CoreLib.Component.ComponentBase;

namespace BudgetLambda.Server.Pages
{
    /// <summary>
    /// Component Mangement page, contains information of all components currently in a package.
    /// </summary>
    public partial class ComponentManagement
    {
        /// <summary>
        /// Parameter for the current package.
        /// </summary>
        [Parameter]
        public PipelinePackage Package { get; set; }
        private ComponentBase SelectedComponent { get; set; }
        private ComponentEditor? editor { get; set; }

        //OK, this is a classic case of boilerplate code. Since the deadline is in two weeks we'll leave it as-is
        private List<(string id, Func<Task> callback)> CreationCallbacks =>
            new()
            {
                ("Lambda Map - C#", CreateComponent<CSharpLambdaMap>),
                ("Source - Http", CreateComponent<HttpSource>),
                ("Sink - Stdout", CreateComponent<StdoutSink>),
            };


        private async Task CreateComponent<T>() where T : ComponentBase, new()
        {
            T newComponent = new() { ComponentName = "New Component" };
            this.Package.ChildComponents.Add(newComponent);
            if (newComponent is ISource)
            {
                this.Package.Source = newComponent;
            }
            database.Components.Add(newComponent);
            await database.SaveChangesAsync();
            await this.LoadComponent(newComponent);
        }

        private async Task LoadComponent(ComponentBase component)
        {
            this.SelectedComponent = component;
            if (editor is not null)
            {
                await editor.ReloadPageAsync();
            }

        }

        private async Task RemoveComponent(ComponentBase component)
        {
            database.Components.Remove(component);
            if (this.SelectedComponent == component) this.SelectedComponent = null;
            await database.SaveChangesAsync();
        }
    }
}
