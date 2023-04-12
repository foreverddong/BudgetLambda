
using BudgetLambda.CoreLib.Component;
using BudgetLambda.CoreLib.Component.Map;
using BudgetLambda.CoreLib.Component.Sink;
using BudgetLambda.CoreLib.Component.Source;
//using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Database
{
    /// <summary>
    /// EF Core Context for all BudgetLambda access
    /// </summary>
    public class BudgetContext : DbContext
    {
        /// <summary>
        /// All DataSchemas
        /// </summary>
        public DbSet<DataSchema> DataSchemas { get; set; }
        /// <summary>
        /// All Components
        /// </summary>
        public DbSet<ComponentBase> Components { get; set; }
        /// <summary>
        /// All Pipeline Packages
        /// </summary>
        public DbSet<PipelinePackage> PipelinePackages { get; set; }
        /// <summary>
        /// All Lambda Map functions in C#
        /// </summary>
        public DbSet<CSharpLambdaMap> CSharpLambdaMaps { get; set; }
        /// <summary>
        /// All Stdout Sinks
        /// </summary>
        public DbSet<StdoutSink> StdoutSinks { get; set; }
        /// <summary>
        /// All HTTP Sources
        /// </summary>
        public DbSet<HttpSource> HttpSources { get; set; }
        /// <summary>
        /// All Property Definitions for Schemas
        /// </summary>
        public DbSet<PropertyDefinition> PropertyDefinitions { get; set; }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseNpgsql(@"Host=192.168.50.6;Username=postgres;Password=postgres;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Budget");
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
