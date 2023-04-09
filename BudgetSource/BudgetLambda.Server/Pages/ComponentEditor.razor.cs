using BudgetLambda.CoreLib.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MudBlazor;
using static MudBlazor.CategoryTypes;
using ComponentBase = BudgetLambda.CoreLib.Component.ComponentBase;
using BudgetLambda.CoreLib.Component.Map;
using BudgetLambda.CoreLib.Component.Sink;
using BudgetLambda.CoreLib.Component.Source;

namespace BudgetLambda.Server.Pages
{
    public partial class ComponentEditor
    {
        [Parameter]
        public Guid packageid { get; set; }
        [Parameter]
        public Guid? componentid { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState>? authenticationState { get; set; }

        private ComponentBase? component;

        private ClaimsPrincipal User { get; set; }
        private PipelinePackage? package;
        
        private bool created;
        MudForm form;
        private ComponentType componentType;
        private String componentName;
        private List<ComponentBase> orphanComponents;
        protected bool selectedMap;

        protected override async Task OnInitializedAsync()
        {
            var auth = await authenticationState;
            this.User = auth.User;
            var res = (from p in database.PipelinePackages
                       where p.PackageID == packageid
                       select p).ToList();
            this.package = res.First();
            this.orphanComponents = package.FindOrphanedComponents();
            if (componentid is null)
            {
                this.created = false;
                this.componentid = Guid.NewGuid();
                this.componentName = "";
            } else
            {
                this.created = true;
                this.component = (from c in database.Components
                                  where c.ComponentID == componentid
                                  select c
                                  ).First();
                this.componentName = this.component.ComponentName;
                this.componentType = this.component.Type;
                if (this.componentType == ComponentType.Map)
                {
                    this.selectedMap = true;
                }
            }
        }

        private async Task<ComponentBase> OnSubmit()
        {
            await form.Validate();
            if (!form.IsValid) return null;

            if (componentType == ComponentType.Source)
            {
                var newComponent = new HttpSource
                {

                    ComponentName = componentName,
                };
                database.Components.Add(newComponent);
                await database.SaveChangesAsync();
                return newComponent;
            } else if (componentType == ComponentType.Map)
            {
                var newComponent = new CSharpLambdaMap
                {
                    ComponentName = componentName,
                };
                database.Components.Add(newComponent);
                await database.SaveChangesAsync();
                return newComponent;
            } else
            {
                var newComponent = new StdoutSink
                {
                    ComponentName = componentName,
                };
                database.Components.Add(newComponent);
                await database.SaveChangesAsync();
                return newComponent;
            }
        }

        private void OnSelectMap()
        {
            this.selectedMap = true;
        }

        private void OnSelectNotMap()
        {
            this.selectedMap = false;
        }
    }
}
