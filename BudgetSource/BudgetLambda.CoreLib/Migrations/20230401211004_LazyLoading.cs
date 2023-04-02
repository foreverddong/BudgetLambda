using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetLambda.CoreLib.Migrations
{
    /// <inheritdoc />
    public partial class LazyLoading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PipelinePackagePackageID",
                schema: "Budget",
                table: "Components",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Components_PipelinePackagePackageID",
                schema: "Budget",
                table: "Components",
                column: "PipelinePackagePackageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_PipelinePackages_PipelinePackagePackageID",
                schema: "Budget",
                table: "Components",
                column: "PipelinePackagePackageID",
                principalSchema: "Budget",
                principalTable: "PipelinePackages",
                principalColumn: "PackageID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_PipelinePackages_PipelinePackagePackageID",
                schema: "Budget",
                table: "Components");

            migrationBuilder.DropIndex(
                name: "IX_Components_PipelinePackagePackageID",
                schema: "Budget",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "PipelinePackagePackageID",
                schema: "Budget",
                table: "Components");
        }
    }
}
