using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetLambda.CoreLib.Migrations
{
    /// <inheritdoc />
    public partial class NullableContents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_DataSchemas_InputSchemaSchemaID",
                schema: "Budget",
                table: "Components");

            migrationBuilder.DropForeignKey(
                name: "FK_Components_DataSchemas_OutputSchemaSchemaID",
                schema: "Budget",
                table: "Components");

            migrationBuilder.AlterColumn<Guid>(
                name: "OutputSchemaSchemaID",
                schema: "Budget",
                table: "Components",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "InputSchemaSchemaID",
                schema: "Budget",
                table: "Components",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_DataSchemas_InputSchemaSchemaID",
                schema: "Budget",
                table: "Components",
                column: "InputSchemaSchemaID",
                principalSchema: "Budget",
                principalTable: "DataSchemas",
                principalColumn: "SchemaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_DataSchemas_OutputSchemaSchemaID",
                schema: "Budget",
                table: "Components",
                column: "OutputSchemaSchemaID",
                principalSchema: "Budget",
                principalTable: "DataSchemas",
                principalColumn: "SchemaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_DataSchemas_InputSchemaSchemaID",
                schema: "Budget",
                table: "Components");

            migrationBuilder.DropForeignKey(
                name: "FK_Components_DataSchemas_OutputSchemaSchemaID",
                schema: "Budget",
                table: "Components");

            migrationBuilder.AlterColumn<Guid>(
                name: "OutputSchemaSchemaID",
                schema: "Budget",
                table: "Components",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "InputSchemaSchemaID",
                schema: "Budget",
                table: "Components",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Components_DataSchemas_InputSchemaSchemaID",
                schema: "Budget",
                table: "Components",
                column: "InputSchemaSchemaID",
                principalSchema: "Budget",
                principalTable: "DataSchemas",
                principalColumn: "SchemaID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Components_DataSchemas_OutputSchemaSchemaID",
                schema: "Budget",
                table: "Components",
                column: "OutputSchemaSchemaID",
                principalSchema: "Budget",
                principalTable: "DataSchemas",
                principalColumn: "SchemaID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
