using BudgetLambda.CoreLib.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using System.Security.Claims;
using ComponentBase = BudgetLambda.CoreLib.Component.ComponentBase;

namespace BudgetLambda.Server.Pages
{
    public partial class ManageComponent
    {
        [Parameter]
        public Guid packageid { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState>? authenticationState { get; set; }
        private ClaimsPrincipal User { get; set; }
        private PipelinePackage? package;
        private List<ComponentBase>? components;

        protected override async Task OnInitializedAsync()
        {
            this.User = (await authenticationState).User;
            var res =  (from p in database.PipelinePackages
                        where p.PackageID == packageid
                        select p).ToList();
            this.package = res.First();
            this.components = this.package.ChildComponents;
        }

        private void RowClicked(TableRowClickEventArgs<ComponentBase> args)
        {
            var componentid = args.Item.ComponentID;

            navigation.NavigateTo($"/packageeditor/{this.package.PackageID}/manage-component/edit/{componentid}", true);
        }

        private void CreateClicked()
        {
            navigation.NavigateTo($"/packageeditor/{this.package.PackageID}/manage-component/edit", true);
        }
    }
}
