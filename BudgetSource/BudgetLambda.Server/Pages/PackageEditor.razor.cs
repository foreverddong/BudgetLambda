using BudgetLambda.CoreLib.Component;
using BudgetLambda.CoreLib.Utility.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text;
using ComponentBase = BudgetLambda.CoreLib.Component.ComponentBase;

namespace BudgetLambda.Server.Pages
{
    public partial class PackageEditor
    {
        [Parameter]
        public Guid? packageid { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState>? authenticationState { get; set; }
        private ClaimsPrincipal User { get; set; }
        private PipelinePackage? package;
        private bool creation = false;
        private bool healthy => this.healthStatus.All(s => s.status);
        private bool processing = true;

        private bool creating { get; set; } = false;
        private int process { get; set; } = 0;
        private string saveText => creation ? "Confirm Creation" : "Save Pipeline";
        private List<(ComponentBase me, bool status, string message)> healthStatus = new();

        private string mermaidDefinition { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            //await interop.InvokeVoidAsync("undef");
            this.User = (await authenticationState).User;
            if (packageid is null)
            {
                this.package = new PipelinePackage();
                this.package.Tenant = this.User.Identity.Name;
                creation = true;
            }
            else
            {
                var res = (from p in database.PipelinePackages
                           where p.PackageID == packageid
                           select p).ToList();
                this.package = res.First();
            }
            this.healthStatus = await package.CheckHealth(client);
            this.processing = false;
            this.mermaidDefinition = this.GenerateDiagram();
            StateHasChanged();

        }

        private async Task SavePackage()
        {
            if (creation)
            {
                database.PipelinePackages.Add(this.package);
            }
            await database.SaveChangesAsync();
            navigation.NavigateTo($"/packageeditor/{this.package.PackageID}/", true);
        }

        private async Task RedeployPipeline()
        {
            this.process = 0;
            this.creating = true;
            await package.PurgePipeline(client);
            scheduler.LoadPackage(package);
            await scheduler.ConfigureMQ();
            await scheduler.SchedulePackage($"{Path.GetTempPath()}budget-{package.PackageName}-{Guid.NewGuid().ShortID()}/", (inc) => { this.process += inc; });
            this.creating = false;
        }

        private async Task PurgePipeline()
        {
            this.process = 0;
            this.creating = true;
            await package.PurgePipeline(client);
            this.creating = false;
        }

        private void OnClickManageComponent()
        {
            navigation.NavigateTo($"/packageeditor/{this.package.PackageID}/manage-component");
        }

        private string GenerateDiagram()
        {
            var builder = new StringBuilder();
            builder.AppendLine("flowchart LR;");
            builder.AppendLine(this.healthStatus
                .Select(s => $"{s.me.ComponentID}[{s.me.ComponentName}<br/>{s.me.Type}<br/>healthy: {s.status}]")
                .Aggregate("",(a, b) => $"{a}\n{b}"));
            var mermaidConnections =
                this.package.ChildComponents.SelectMany(c => c.Next.Select(sub => $"{c.ComponentID} --> {sub.ComponentID}")).Aggregate(string.Empty, (a, b) => $"{a}\n{b}");
            builder.AppendLine(mermaidConnections);
            return builder.ToString();
        }

        private async Task DeletePackage()
        {
            database.PropertyDefinitions.RemoveRange(this.package.Schamas.SelectMany(s => s.Mapping));
            await database.SaveChangesAsync();
            database.DataSchemas.RemoveRange(this.package.Schamas);
            await database.SaveChangesAsync();
            database.PipelinePackages.Remove(this.package);
            await database.SaveChangesAsync();
            navigation.NavigateTo("/");
        }
    }
}
