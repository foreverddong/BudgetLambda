using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetLambda.CoreLib.Migrations
{
    /// <inheritdoc />
    public partial class PopulateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Budget");

            migrationBuilder.CreateTable(
                name: "DataSchemas",
                schema: "Budget",
                columns: table => new
                {
                    SchemaID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSchemas", x => x.SchemaID);
                });

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

            migrationBuilder.CreateTable(
                name: "Components",
                schema: "Budget",
                columns: table => new
                {
                    ComponentID = table.Column<Guid>(type: "uuid", nullable: false),
                    ComponentName = table.Column<string>(type: "text", nullable: false),
                    InputSchemaSchemaID = table.Column<Guid>(type: "uuid", nullable: false),
                    OutputSchemaSchemaID = table.Column<Guid>(type: "uuid", nullable: false),
                    ComponentBaseComponentID = table.Column<Guid>(type: "uuid", nullable: true),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    ExchangeName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.ComponentID);
                    table.ForeignKey(
                        name: "FK_Components_Components_ComponentBaseComponentID",
                        column: x => x.ComponentBaseComponentID,
                        principalSchema: "Budget",
                        principalTable: "Components",
                        principalColumn: "ComponentID");
                    table.ForeignKey(
                        name: "FK_Components_DataSchemas_InputSchemaSchemaID",
                        column: x => x.InputSchemaSchemaID,
                        principalSchema: "Budget",
                        principalTable: "DataSchemas",
                        principalColumn: "SchemaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Components_DataSchemas_OutputSchemaSchemaID",
                        column: x => x.OutputSchemaSchemaID,
                        principalSchema: "Budget",
                        principalTable: "DataSchemas",
                        principalColumn: "SchemaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyDefinitions",
                schema: "Budget",
                columns: table => new
                {
                    DefinitionID = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Identifier = table.Column<string>(type: "text", nullable: false),
                    DataSchemaSchemaID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyDefinitions", x => x.DefinitionID);
                    table.ForeignKey(
                        name: "FK_PropertyDefinitions_DataSchemas_DataSchemaSchemaID",
                        column: x => x.DataSchemaSchemaID,
                        principalSchema: "Budget",
                        principalTable: "DataSchemas",
                        principalColumn: "SchemaID");
                });

            migrationBuilder.CreateTable(
                name: "PipelinePackages",
                schema: "Budget",
                columns: table => new
                {
                    PackageID = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantID = table.Column<Guid>(type: "uuid", nullable: false),
                    PackageName = table.Column<string>(type: "text", nullable: false),
                    SourceComponentID = table.Column<Guid>(type: "uuid", nullable: false),
                    ExchangeName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PipelinePackages", x => x.PackageID);
                    table.ForeignKey(
                        name: "FK_PipelinePackages_Components_SourceComponentID",
                        column: x => x.SourceComponentID,
                        principalSchema: "Budget",
                        principalTable: "Components",
                        principalColumn: "ComponentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PipelinePackages_Tenants_TenantID",
                        column: x => x.TenantID,
                        principalSchema: "Budget",
                        principalTable: "Tenants",
                        principalColumn: "TenantID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Components_ComponentBaseComponentID",
                schema: "Budget",
                table: "Components",
                column: "ComponentBaseComponentID");

            migrationBuilder.CreateIndex(
                name: "IX_Components_InputSchemaSchemaID",
                schema: "Budget",
                table: "Components",
                column: "InputSchemaSchemaID");

            migrationBuilder.CreateIndex(
                name: "IX_Components_OutputSchemaSchemaID",
                schema: "Budget",
                table: "Components",
                column: "OutputSchemaSchemaID");

            migrationBuilder.CreateIndex(
                name: "IX_PipelinePackages_SourceComponentID",
                schema: "Budget",
                table: "PipelinePackages",
                column: "SourceComponentID");

            migrationBuilder.CreateIndex(
                name: "IX_PipelinePackages_TenantID",
                schema: "Budget",
                table: "PipelinePackages",
                column: "TenantID");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDefinitions_DataSchemaSchemaID",
                schema: "Budget",
                table: "PropertyDefinitions",
                column: "DataSchemaSchemaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PipelinePackages",
                schema: "Budget");

            migrationBuilder.DropTable(
                name: "PropertyDefinitions",
                schema: "Budget");

            migrationBuilder.DropTable(
                name: "Components",
                schema: "Budget");

            migrationBuilder.DropTable(
                name: "Tenants",
                schema: "Budget");

            migrationBuilder.DropTable(
                name: "DataSchemas",
                schema: "Budget");
        }
    }
}
