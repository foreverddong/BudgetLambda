using BlazorMonaco;
using BlazorMonaco.Editor;
using BudgetLambda.CoreLib.Component.Map;
using Microsoft.AspNetCore.Components;
using BudgetLambda.CoreLib.Component.Interfaces;
using ComponentBase = BudgetLambda.CoreLib.Component.ComponentBase;
using BudgetLambda.CoreLib.Component;
using MudBlazor;
using System.Text.Json;
using BudgetLambda.CoreLib.Utility.Faas;

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
        private int currentReplicas { get; set; } = 0;
        private int targetReplicas { get; set; } = 0;
        private bool componentOnline = false;


        private bool inputSchemaDisabled => (Component is ISource);
        private bool outputSchemaDisabled => (Component is ISink);

        private bool nextComponentDisabled => (Component is ISink) || (this.Package.FindOrphanedComponents().Contains(this.Component));

        private string logSrc => $"http://192.168.52.9/d-solo/5on-IIE4k/budgetlogs?orgId=1&var-jobname=openfaas-fn%2F{this.Component.ServiceName}&panelId=2";

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
            await this.ObtainCurrentRelicas();
            if (dropContainer is not null)
            {
                dropContainer.Refresh();
            }
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

        private async Task ObtainCurrentRelicas()
        {
            if (this.componentOnline is false) return;
            var info = await client.FunctionGETAsync(this.Component.ServiceName);
            var replicaCount = Convert.ToInt32(info.Replicas);
            this.currentReplicas = replicaCount;
        }

        private async Task ScaleReplicas()
        {
            var count = this.targetReplicas;
            using var stream = new MemoryStream();
            JsonSerializer.Serialize(stream, new
            {
                service = this.Component.ServiceName,
                replicas = count
            });
            stream.Seek(0, SeekOrigin.Begin);
            await client.ScaleFunctionAsync(this.Component.ServiceName, stream );
            await this.ObtainCurrentRelicas();
        }

        private async Task CheckComponentOnline()
        {
            try
            {
                var info = await client.FunctionGETAsync(this.Component.ServiceName);
                this.componentOnline = true;
            }
            catch (FaasClientException ex)
            {
                this.componentOnline = false;
            }
        }

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                await this.CheckComponentOnline();
                await this.ReloadPageAsync();
            }
            base.OnAfterRender(firstRender);
        }

    }
}
