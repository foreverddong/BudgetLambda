﻿// <auto-generated />
using System;
using BudgetLambda.CoreLib.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BudgetLambda.CoreLib.Migrations
{
    [DbContext(typeof(BudgetContext))]
    partial class BudgetContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Budget")
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.ComponentBase", b =>
                {
                    b.Property<Guid>("ComponentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ComponentName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("InputKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("InputSchemaSchemaID")
                        .HasColumnType("uuid");

                    b.Property<string>("OutputKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OutputSchemaSchemaID")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("PipelinePackagePackageID")
                        .HasColumnType("uuid");

                    b.HasKey("ComponentID");

                    b.HasIndex("InputSchemaSchemaID");

                    b.HasIndex("OutputSchemaSchemaID");

                    b.HasIndex("PipelinePackagePackageID");

                    b.ToTable("Components", "Budget");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ComponentBase");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.DataSchema", b =>
                {
                    b.Property<Guid>("SchemaID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("SchemaName")
                        .HasColumnType("text");

                    b.HasKey("SchemaID");

                    b.ToTable("DataSchemas", "Budget");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.PipelinePackage", b =>
                {
                    b.Property<Guid>("PackageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("PackageName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("SchamasSchemaID")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("SourceComponentID")
                        .HasColumnType("uuid");

                    b.Property<string>("Tenant")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("PackageID");

                    b.HasIndex("SchamasSchemaID");

                    b.HasIndex("SourceComponentID");

                    b.ToTable("PipelinePackages", "Budget");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.PropertyDefinition", b =>
                {
                    b.Property<Guid>("DefinitionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DataSchemaSchemaID")
                        .HasColumnType("uuid");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("DefinitionID");

                    b.HasIndex("DataSchemaSchemaID");

                    b.ToTable("PropertyDefinitions", "Budget");
                });

            modelBuilder.Entity("ComponentBaseComponentBase", b =>
                {
                    b.Property<Guid>("AllChildComponentsComponentID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("NextComponentID")
                        .HasColumnType("uuid");

                    b.HasKey("AllChildComponentsComponentID", "NextComponentID");

                    b.HasIndex("NextComponentID");

                    b.ToTable("ComponentBaseComponentBase", "Budget");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.Map.CSharpLambdaMap", b =>
                {
                    b.HasBaseType("BudgetLambda.CoreLib.Component.ComponentBase");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("CSharpLambdaMap");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.Source.RabbitMQSource", b =>
                {
                    b.HasBaseType("BudgetLambda.CoreLib.Component.ComponentBase");

                    b.Property<string>("ExchangeName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("RabbitMQSource");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.ComponentBase", b =>
                {
                    b.HasOne("BudgetLambda.CoreLib.Component.DataSchema", "InputSchema")
                        .WithMany()
                        .HasForeignKey("InputSchemaSchemaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgetLambda.CoreLib.Component.DataSchema", "OutputSchema")
                        .WithMany()
                        .HasForeignKey("OutputSchemaSchemaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgetLambda.CoreLib.Component.PipelinePackage", null)
                        .WithMany("ChildComponents")
                        .HasForeignKey("PipelinePackagePackageID");

                    b.Navigation("InputSchema");

                    b.Navigation("OutputSchema");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.PipelinePackage", b =>
                {
                    b.HasOne("BudgetLambda.CoreLib.Component.DataSchema", "Schamas")
                        .WithMany()
                        .HasForeignKey("SchamasSchemaID");

                    b.HasOne("BudgetLambda.CoreLib.Component.ComponentBase", "Source")
                        .WithMany()
                        .HasForeignKey("SourceComponentID");

                    b.Navigation("Schamas");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.PropertyDefinition", b =>
                {
                    b.HasOne("BudgetLambda.CoreLib.Component.DataSchema", null)
                        .WithMany("Mapping")
                        .HasForeignKey("DataSchemaSchemaID");
                });

            modelBuilder.Entity("ComponentBaseComponentBase", b =>
                {
                    b.HasOne("BudgetLambda.CoreLib.Component.ComponentBase", null)
                        .WithMany()
                        .HasForeignKey("AllChildComponentsComponentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgetLambda.CoreLib.Component.ComponentBase", null)
                        .WithMany()
                        .HasForeignKey("NextComponentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.DataSchema", b =>
                {
                    b.Navigation("Mapping");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.PipelinePackage", b =>
                {
                    b.Navigation("ChildComponents");
                });
#pragma warning restore 612, 618
        }
    }
}
