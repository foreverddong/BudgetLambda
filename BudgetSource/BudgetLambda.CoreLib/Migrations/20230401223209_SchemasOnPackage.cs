using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetLambda.CoreLib.Migrations
{
    /// <inheritdoc />
    public partial class SchemasOnPackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SchamasSchemaID",
                schema: "Budget",
                table: "PipelinePackages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
                principalColumn: "SchemaID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
