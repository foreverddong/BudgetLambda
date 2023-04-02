using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetLambda.CoreLib.Migrations
{
    /// <inheritdoc />
    public partial class NullablePackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PipelinePackages_Components_SourceComponentID",
                schema: "Budget",
                table: "PipelinePackages");

            migrationBuilder.DropForeignKey(
                name: "FK_PipelinePackages_DataSchemas_SchamasSchemaID",
                schema: "Budget",
                table: "PipelinePackages");

            migrationBuilder.DropForeignKey(
                name: "FK_PipelinePackages_Tenants_TenantID",
                schema: "Budget",
                table: "PipelinePackages");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantID",
                schema: "Budget",
                table: "PipelinePackages",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "SourceComponentID",
                schema: "Budget",
                table: "PipelinePackages",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "SchamasSchemaID",
                schema: "Budget",
                table: "PipelinePackages",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_PipelinePackages_Components_SourceComponentID",
                schema: "Budget",
                table: "PipelinePackages",
                column: "SourceComponentID",
                principalSchema: "Budget",
                principalTable: "Components",
                principalColumn: "ComponentID");

            migrationBuilder.AddForeignKey(
                name: "FK_PipelinePackages_DataSchemas_SchamasSchemaID",
                schema: "Budget",
                table: "PipelinePackages",
                column: "SchamasSchemaID",
                principalSchema: "Budget",
                principalTable: "DataSchemas",
                principalColumn: "SchemaID");

            migrationBuilder.AddForeignKey(
                name: "FK_PipelinePackages_Tenants_TenantID",
                schema: "Budget",
                table: "PipelinePackages",
                column: "TenantID",
                principalSchema: "Budget",
                principalTable: "Tenants",
                principalColumn: "TenantID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PipelinePackages_Components_SourceComponentID",
                schema: "Budget",
                table: "PipelinePackages");

            migrationBuilder.DropForeignKey(
                name: "FK_PipelinePackages_DataSchemas_SchamasSchemaID",
                schema: "Budget",
                table: "PipelinePackages");

            migrationBuilder.DropForeignKey(
                name: "FK_PipelinePackages_Tenants_TenantID",
                schema: "Budget",
                table: "PipelinePackages");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantID",
                schema: "Budget",
                table: "PipelinePackages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SourceComponentID",
                schema: "Budget",
                table: "PipelinePackages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SchamasSchemaID",
                schema: "Budget",
                table: "PipelinePackages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PipelinePackages_Components_SourceComponentID",
                schema: "Budget",
                table: "PipelinePackages",
                column: "SourceComponentID",
                principalSchema: "Budget",
                principalTable: "Components",
                principalColumn: "ComponentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PipelinePackages_DataSchemas_SchamasSchemaID",
                schema: "Budget",
                table: "PipelinePackages",
                column: "SchamasSchemaID",
                principalSchema: "Budget",
                principalTable: "DataSchemas",
                principalColumn: "SchemaID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PipelinePackages_Tenants_TenantID",
                schema: "Budget",
                table: "PipelinePackages",
                column: "TenantID",
                principalSchema: "Budget",
                principalTable: "Tenants",
                principalColumn: "TenantID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
