using BlazorMonaco;
using BlazorMonaco.Editor;
using BudgetLambda.CoreLib.Component.Map;
using Microsoft.AspNetCore.Components;
using BudgetLambda.CoreLib.Component.Interfaces;
using ComponentBase = BudgetLambda.CoreLib.Component.ComponentBase;
using BudgetLambda.CoreLib.Component;

namespace BudgetLambda.Server.Pages
{
    public partial class ComponentEditor
    {
        [Parameter]
        public ComponentBase Component { get; set; }
        [Parameter]
        public PipelinePackage Package { get; set; }

        private StandaloneCodeEditor? editor { get; set; }

        private bool inputSchemaDisabled => (Component is ISource);
        private bool outputSchemaDisabled => (Component is ISink);

        private StandaloneEditorConstructionOptions CSharpEditorOptions(StandaloneCodeEditor editor)
        {
            return new StandaloneEditorConstructionOptions
            {
                AutomaticLayout = true,
                Language = "csharp",
                Value = """
                public partial class Handler
                {
                    public OutputModel HandleData(InputModel data)
                    {
                        return new OutputModel();
                    }
                }
                """,
            };
        }

        private async Task UpdateCodeValue(KeyboardEvent e)
        {
            var model = await editor.GetModel();
            var code = await model.GetValue(EndOfLinePreference.TextDefined, true);
            if (Component is ILambdaMap c)
            {
                c.Code = code;
            }
        }

        public async Task ReloadPageAsync()
        {
            if (Component is ILambdaMap c)
            {
                var model = await editor.GetModel();
                await model.SetValue(c.Code);
            }
        }

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Delay(500);
                await this.ReloadPageAsync();
            }
            base.OnAfterRender(firstRender);
        }

    }
}
