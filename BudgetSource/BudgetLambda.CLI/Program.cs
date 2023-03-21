using BudgetLambda.CoreLib.Business;
using BudgetLambda.CoreLib.Component;
using BudgetLambda.CoreLib.Scheduler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace BudgetLambda.CLI
{
    public class Program
    {
        public IServiceProvider Services { get; set; }

        static void Main(string[] args)
        {
            
            new Program().MainAsync(args, ConfigureServices()).GetAwaiter().GetResult();
        }

        public static IServiceProvider ConfigureServices()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var provider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddLogging(c => c.AddSimpleConsole())
                .AddSingleton<BudgetWorkloadScheduler>();
                
            return provider.BuildServiceProvider();
        }

        public async Task MainAsync(string[] args, IServiceProvider _services)
        {
            this.Services = _services;

            var scheduler = Services.GetRequiredService<BudgetWorkloadScheduler>();
            var pack = this.BuildSamplePipeline();
            await scheduler.ConfigureMQ(pack);

        }

        public PipelinePackage BuildSamplePipeline()
        {
            var SampleTenant = new BudgetTenant
            {
                TenantID = Guid.Parse("41cc569f-9e9d-4bcb-8182-0a46ebd3d19c"),
                TenantName = "BudgetOrg",
            };

            //41cc569f-SamplePipeline
            var package = new PipelinePackage
            {
                PackageID = Guid.NewGuid(),
                ExchangeName = "SamplePipeline",
                PackageName = "SamplePipeline",
                Source = null,
                Tenant = SampleTenant,
            };

            return package;
        }
    }
}