using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetLambda.CoreLib.Migrations
{
    /// <inheritdoc />
    public partial class WorkingComponent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Components_ComponentBaseComponentID",
                schema: "Budget",
                table: "Components");

            migrationBuilder.DropIndex(
                name: "IX_Components_ComponentBaseComponentID",
                schema: "Budget",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "ExchangeName",
                schema: "Budget",
                table: "PipelinePackages");

            migrationBuilder.DropColumn(
                name: "ComponentBaseComponentID",
                schema: "Budget",
                table: "Components");

            migrationBuilder.CreateTable(
                name: "ComponentBaseComponentBase",
                schema: "Budget",
                columns: table => new
                {
                    AllChildComponentsComponentID = table.Column<Guid>(type: "uuid", nullable: false),
                    NextComponentID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentBaseComponentBase", x => new { x.AllChildComponentsComponentID, x.NextComponentID });
                    table.ForeignKey(
                        name: "FK_ComponentBaseComponentBase_Components_AllChildComponentsCom~",
                        column: x => x.AllChildComponentsComponentID,
                        principalSchema: "Budget",
                        principalTable: "Components",
                        principalColumn: "ComponentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComponentBaseComponentBase_Components_NextComponentID",
                        column: x => x.NextComponentID,
                        principalSchema: "Budget",
                        principalTable: "Components",
                        principalColumn: "ComponentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentBaseComponentBase_NextComponentID",
                schema: "Budget",
                table: "ComponentBaseComponentBase",
                column: "NextComponentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentBaseComponentBase",
                schema: "Budget");

            migrationBuilder.AddColumn<string>(
                name: "ExchangeName",
                schema: "Budget",
                table: "PipelinePackages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ComponentBaseComponentID",
                schema: "Budget",
                table: "Components",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Components_ComponentBaseComponentID",
                schema: "Budget",
                table: "Components",
                column: "ComponentBaseComponentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Components_ComponentBaseComponentID",
                schema: "Budget",
                table: "Components",
                column: "ComponentBaseComponentID",
                principalSchema: "Budget",
                principalTable: "Components",
                principalColumn: "ComponentID");
        }
    }
}
