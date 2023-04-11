using BudgetLambda.CoreLib.Component;
using BudgetLambda.CoreLib.Component.Map;
using BudgetLambda.CoreLib.Component.Sink;
using BudgetLambda.CoreLib.Component.Source;
using Microsoft.AspNetCore.Components;
using ComponentBase = BudgetLambda.CoreLib.Component.ComponentBase;

namespace BudgetLambda.Server.Pages
{
    public partial class ComponentManagement
    {
        [Parameter]
        public PipelinePackage Package { get; set; }
        public ComponentBase SelectedComponent { get; set; }

        //OK, this is a classic case of boilerplate code. Since the deadline is in two weeks we'll leave it as-is
        private List<(string id, Func<Task> callback)> CreationCallbacks =>
            new()
            {
                ("Lambda Map - C#", CreateComponent<CSharpLambdaMap>),
                ("Source - Http", CreateComponent<HttpSource>),
                ("Sink - Stdout", CreateComponent<StdoutSink>),
            };


        public async Task CreateComponent<T>() where T : ComponentBase, new()
        {
            T newComponent = new();
            database.Components.Add(newComponent);
            await database.SaveChangesAsync();
        }

        public async Task LoadComponent(ComponentBase component)
        {
            this.SelectedComponent = component;
        }
    }
}
