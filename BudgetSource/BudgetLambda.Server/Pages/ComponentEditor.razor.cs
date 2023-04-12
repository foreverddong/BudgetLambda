using BlazorMonaco;
using BlazorMonaco.Editor;
using BudgetLambda.CoreLib.Component.Map;
using Microsoft.AspNetCore.Components;
using BudgetLambda.CoreLib.Component.Interfaces;
using ComponentBase = BudgetLambda.CoreLib.Component.ComponentBase;
using BudgetLambda.CoreLib.Component;
using MudBlazor;

namespace BudgetLambda.Server.Pages
{
    public partial class ComponentEditor
    {
        [Parameter]
        public ComponentBase Component { get; set; }
        [Parameter]
        public PipelinePackage Package { get; set; }

        private StandaloneCodeEditor? editor { get; set; }
        private MudDropContainer<ComponentBase>? dropContainer { get; set; }

        private bool inputSchemaDisabled => (Component is ISource);
        private bool outputSchemaDisabled => (Component is ISink);

        private bool nextComponentDisabled => (Component is ISink) || (this.Package.FindOrphanedComponents().Contains(this.Component));

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
            var code = await editor.GetValue();
            if (Component is ILambdaMap c)
            {
                c.Code = code;
            }
            await database.SaveChangesAsync();
        }

        public async Task ReloadPageAsync()
        {
            //This is another one of those really stupid workarounds.
            await Task.Delay(100);
            if (Component is ILambdaMap c)
            {
                await editor.SetValue(c.Code);
            }
            dropContainer.Refresh();
            StateHasChanged();
        }

        private bool DropSelector(ComponentBase item, string dropid)
        {
            var reachableItems = this.Package.Source.AllChildComponents();
            return dropid switch
            {
                "avaliable" => (!reachableItems.Contains(item) && this.Component != item),
                "selected" => (this.Component.Next.Contains(item)),
                _ => false,
            };

        }

        private async Task ItemUpdated(MudItemDropInfo<ComponentBase> dropItem)
        {
            if (dropItem.DropzoneIdentifier == "selected")
            {
                if (!this.Component.Next.Contains(dropItem.Item))
                {
                    this.Component.Next.Add(dropItem.Item);
                }

            }
            else if (dropItem.DropzoneIdentifier == "avaliable")
            {
                if (this.Component.Next.Contains(dropItem.Item))
                {
                    this.Component.Next.Remove(dropItem.Item);
                    dropItem.Item.DetachChildComponents();
                }
            }
            await database.SaveChangesAsync();
        }

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                await this.ReloadPageAsync();
            }
            base.OnAfterRender(firstRender);
        }

    }
}
