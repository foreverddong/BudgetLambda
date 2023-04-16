﻿// <auto-generated />
using System;
using BudgetLambda.CoreLib.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BudgetLambda.CoreLib.Migrations
{
    [DbContext(typeof(BudgetContext))]
    [Migration("20230416054213_EmailSink")]
    partial class EmailSink
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<Guid?>("ComponentBaseComponentID")
                        .HasColumnType("uuid");

                    b.Property<string>("ComponentName")
                        .HasColumnType("text");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("InputKey")
                        .HasColumnType("text");

                    b.Property<Guid?>("InputSchemaSchemaID")
                        .HasColumnType("uuid");

                    b.Property<string>("OutputKey")
                        .HasColumnType("text");

                    b.Property<Guid?>("OutputSchemaSchemaID")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("PipelinePackagePackageID")
                        .HasColumnType("uuid");

                    b.HasKey("ComponentID");

                    b.HasIndex("ComponentBaseComponentID");

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

                    b.Property<Guid?>("PipelinePackagePackageID")
                        .HasColumnType("uuid");

                    b.Property<string>("SchemaName")
                        .HasColumnType("text");

                    b.HasKey("SchemaID");

                    b.HasIndex("PipelinePackagePackageID");

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

                    b.Property<Guid?>("SourceComponentID")
                        .HasColumnType("uuid");

                    b.Property<string>("Tenant")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("PackageID");

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

                    b.Property<bool>("IsList")
                        .HasColumnType("boolean");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("DefinitionID");

                    b.HasIndex("DataSchemaSchemaID");

                    b.ToTable("PropertyDefinitions", "Budget");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.Map.CSharpLambdaMap", b =>
                {
                    b.HasBaseType("BudgetLambda.CoreLib.Component.ComponentBase");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("CSharpLambdaMap");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.Sink.EmailSink", b =>
                {
                    b.HasBaseType("BudgetLambda.CoreLib.Component.ComponentBase");

                    b.HasDiscriminator().HasValue("EmailSink");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.Sink.StdoutSink", b =>
                {
                    b.HasBaseType("BudgetLambda.CoreLib.Component.ComponentBase");

                    b.HasDiscriminator().HasValue("StdoutSink");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.Source.HttpSource", b =>
                {
                    b.HasBaseType("BudgetLambda.CoreLib.Component.ComponentBase");

                    b.HasDiscriminator().HasValue("HttpSource");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.ComponentBase", b =>
                {
                    b.HasOne("BudgetLambda.CoreLib.Component.ComponentBase", null)
                        .WithMany("Next")
                        .HasForeignKey("ComponentBaseComponentID");

                    b.HasOne("BudgetLambda.CoreLib.Component.DataSchema", "InputSchema")
                        .WithMany()
                        .HasForeignKey("InputSchemaSchemaID");

                    b.HasOne("BudgetLambda.CoreLib.Component.DataSchema", "OutputSchema")
                        .WithMany()
                        .HasForeignKey("OutputSchemaSchemaID");

                    b.HasOne("BudgetLambda.CoreLib.Component.PipelinePackage", null)
                        .WithMany("ChildComponents")
                        .HasForeignKey("PipelinePackagePackageID");

                    b.Navigation("InputSchema");

                    b.Navigation("OutputSchema");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.DataSchema", b =>
                {
                    b.HasOne("BudgetLambda.CoreLib.Component.PipelinePackage", null)
                        .WithMany("Schamas")
                        .HasForeignKey("PipelinePackagePackageID");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.PipelinePackage", b =>
                {
                    b.HasOne("BudgetLambda.CoreLib.Component.ComponentBase", "Source")
                        .WithMany()
                        .HasForeignKey("SourceComponentID");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.PropertyDefinition", b =>
                {
                    b.HasOne("BudgetLambda.CoreLib.Component.DataSchema", null)
                        .WithMany("Mapping")
                        .HasForeignKey("DataSchemaSchemaID");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.ComponentBase", b =>
                {
                    b.Navigation("Next");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.DataSchema", b =>
                {
                    b.Navigation("Mapping");
                });

            modelBuilder.Entity("BudgetLambda.CoreLib.Component.PipelinePackage", b =>
                {
                    b.Navigation("ChildComponents");

                    b.Navigation("Schamas");
                });
#pragma warning restore 612, 618
        }
    }
}
