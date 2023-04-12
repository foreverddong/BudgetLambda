using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using BudgetLambda.CoreLib.Component;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using static MudBlazor.CategoryTypes;

namespace BudgetLambda.Server.Pages
{
    /// <summary>
    /// UI Component to display all packages owned by a user, as well as providing navigation to these packages.
    /// </summary>
    public partial class MainUserInfo
    {
        [CascadingParameter]
        private Task<AuthenticationState>? authenticationState { get; set; }

        private ClaimsPrincipal User { get; set; }
        private List<PipelinePackage> pipelinePackages { get; set; } = new();
/// <inheritdoc/>

        protected override async Task OnInitializedAsync()
        {
            var auth = await authenticationState;
            this.User = auth.User;
            this.pipelinePackages = await (from p in database.PipelinePackages
                                    where p.Tenant == this.User.Identity.Name
                                    select p).ToListAsync();
        }

        private void NavigateToCreation()
        {
            navigation.NavigateTo("/packageeditor", true);
        }

        private async Task CreateSamplePackage()
        {
            var pkg = await sample.BuildCompletePackage(User.Identity.Name, "CompleteSamplePackage");
            database.PipelinePackages.Add(pkg);
            database.SaveChanges();
            navigation.NavigateTo($"/", true);
        }

        private void RowClicked(TableRowClickEventArgs<PipelinePackage> args)
        {
            var targetid = args.Item.PackageID;
            navigation.NavigateTo($"/packageeditor/{targetid}", true);
        }
    }
}
