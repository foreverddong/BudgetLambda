using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetLambda.CoreLib.Migrations
{
    /// <inheritdoc />
    public partial class ListSchemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PipelinePackages_DataSchemas_SchamasSchemaID",
                schema: "Budget",
                table: "PipelinePackages");

            migrationBuilder.DropIndex(
                name: "IX_PipelinePackages_SchamasSchemaID",
                schema: "Budget",
                table: "PipelinePackages");

            migrationBuilder.DropColumn(
                name: "SchamasSchemaID",
                schema: "Budget",
                table: "PipelinePackages");

            migrationBuilder.AddColumn<Guid>(
                name: "PipelinePackagePackageID",
                schema: "Budget",
                table: "DataSchemas",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataSchemas_PipelinePackagePackageID",
                schema: "Budget",
                table: "DataSchemas",
                column: "PipelinePackagePackageID");

            migrationBuilder.AddForeignKey(
                name: "FK_DataSchemas_PipelinePackages_PipelinePackagePackageID",
                schema: "Budget",
                table: "DataSchemas",
                column: "PipelinePackagePackageID",
                principalSchema: "Budget",
                principalTable: "PipelinePackages",
                principalColumn: "PackageID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataSchemas_PipelinePackages_PipelinePackagePackageID",
                schema: "Budget",
                table: "DataSchemas");

            migrationBuilder.DropIndex(
                name: "IX_DataSchemas_PipelinePackagePackageID",
                schema: "Budget",
                table: "DataSchemas");

            migrationBuilder.DropColumn(
                name: "PipelinePackagePackageID",
                schema: "Budget",
                table: "DataSchemas");

            migrationBuilder.AddColumn<Guid>(
                name: "SchamasSchemaID",
                schema: "Budget",
                table: "PipelinePackages",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PipelinePackages_SchamasSchemaID",
                schema: "Budget",
                table: "PipelinePackages",
                column: "SchamasSchemaID");

            migrationBuilder.AddForeignKey(
                name: "FK_PipelinePackages_DataSchemas_SchamasSchemaID",
                schema: "Budget",
                table: "PipelinePackages",
                column: "SchamasSchemaID",
                principalSchema: "Budget",
                principalTable: "DataSchemas",
                principalColumn: "SchemaID");
        }
    }
}
