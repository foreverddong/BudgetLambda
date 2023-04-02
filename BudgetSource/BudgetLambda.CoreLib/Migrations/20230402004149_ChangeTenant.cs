using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetLambda.CoreLib.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PipelinePackages_Tenants_TenantID",
                schema: "Budget",
                table: "PipelinePackages");

            migrationBuilder.DropTable(
                name: "Tenants",
                schema: "Budget");

            migrationBuilder.DropIndex(
                name: "IX_PipelinePackages_TenantID",
                schema: "Budget",
                table: "PipelinePackages");

            migrationBuilder.DropColumn(
                name: "TenantID",
                schema: "Budget",
                table: "PipelinePackages");

            migrationBuilder.AddColumn<string>(
                name: "Tenant",
                schema: "Budget",
                table: "PipelinePackages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tenant",
                schema: "Budget",
                table: "PipelinePackages");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantID",
                schema: "Budget",
                table: "PipelinePackages",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tenants",
                schema: "Budget",
                columns: table => new
                {
                    TenantID = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.TenantID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PipelinePackages_TenantID",
                schema: "Budget",
                table: "PipelinePackages",
                column: "TenantID");

            migrationBuilder.AddForeignKey(
                name: "FK_PipelinePackages_Tenants_TenantID",
                schema: "Budget",
                table: "PipelinePackages",
                column: "TenantID",
                principalSchema: "Budget",
                principalTable: "Tenants",
                principalColumn: "TenantID");
        }
    }
}
