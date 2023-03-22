using BudgetLambda.CoreLib.Business;
using BudgetLambda.CoreLib.Component;
using BudgetLambda.CoreLib.Component.Map;
using BudgetLambda.CoreLib.Component.Source;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Database
{
    public class BudgetContext : DbContext
    {
        public DbSet<BudgetTenant> Tenants { get; set; }
        public DbSet<DataSchema> DataSchemas { get; set; }
        public DbSet<ComponentBase> Components { get; set; }
        public DbSet<PipelinePackage> PipelinePackages { get; set; }
        public DbSet<CSharpLambdaMap> CSharpLambdaMaps { get; set; }
        public DbSet<RabbitMQSource> RabbitMQSources { get; set; }
        public DbSet<PropertyDefinition> PropertyDefinitions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"Host=192.168.50.6;Username=postgres;Password=postgres;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Budget");
        }
    }
}
