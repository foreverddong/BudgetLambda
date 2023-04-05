using BudgetLambda.CoreLib.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        private bool healthy => this.healthStatus.Any(s => s.status == false);
        private bool processing = true;
        private string saveText => creation ? "Confirm Creation" : "Save Pipeline";
        private List<(bool status, string message)> healthStatus = new();

        protected override async Task OnInitializedAsync()
        {
            this.User = (await authenticationState).User;
            if (packageid is null)
            {
                this.package = new PipelinePackage();
                this.package.Tenant = this.User.Identity.Name;
                creation = true;
            }
            else 
            {
                var res =  (from p in database.PipelinePackages
                           where p.PackageID == packageid
                           select p).ToList();
                this.package = res.First();
            }
            this.healthStatus = await package.CheckHealth(client);
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
    }
}
