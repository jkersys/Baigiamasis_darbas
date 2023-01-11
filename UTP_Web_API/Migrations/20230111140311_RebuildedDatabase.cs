using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UTPWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RebuildedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdministrativeInspectionInvestigationStage");

            migrationBuilder.DropTable(
                name: "ComplainInvestigationStage");

            migrationBuilder.DropTable(
                name: "InvestigationInvestigationStage");

            migrationBuilder.AddColumn<int>(
                name: "AdministrativeInspectionId",
                table: "InvestigationStage",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComplainId",
                table: "InvestigationStage",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvestigationId",
                table: "InvestigationStage",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Decision",
                table: "Conclusion",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationStage_AdministrativeInspectionId",
                table: "InvestigationStage",
                column: "AdministrativeInspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationStage_ComplainId",
                table: "InvestigationStage",
                column: "ComplainId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationStage_InvestigationId",
                table: "InvestigationStage",
                column: "InvestigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestigationStage_AdministrativeInspection_AdministrativeInspectionId",
                table: "InvestigationStage",
                column: "AdministrativeInspectionId",
                principalTable: "AdministrativeInspection",
                principalColumn: "AdministrativeInspectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestigationStage_Complain_ComplainId",
                table: "InvestigationStage",
                column: "ComplainId",
                principalTable: "Complain",
                principalColumn: "ComplainId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestigationStage_Investigation_InvestigationId",
                table: "InvestigationStage",
                column: "InvestigationId",
                principalTable: "Investigation",
                principalColumn: "InvestigationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestigationStage_AdministrativeInspection_AdministrativeInspectionId",
                table: "InvestigationStage");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestigationStage_Complain_ComplainId",
                table: "InvestigationStage");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestigationStage_Investigation_InvestigationId",
                table: "InvestigationStage");

            migrationBuilder.DropIndex(
                name: "IX_InvestigationStage_AdministrativeInspectionId",
                table: "InvestigationStage");

            migrationBuilder.DropIndex(
                name: "IX_InvestigationStage_ComplainId",
                table: "InvestigationStage");

            migrationBuilder.DropIndex(
                name: "IX_InvestigationStage_InvestigationId",
                table: "InvestigationStage");

            migrationBuilder.DropColumn(
                name: "AdministrativeInspectionId",
                table: "InvestigationStage");

            migrationBuilder.DropColumn(
                name: "ComplainId",
                table: "InvestigationStage");

            migrationBuilder.DropColumn(
                name: "InvestigationId",
                table: "InvestigationStage");

            migrationBuilder.AlterColumn<string>(
                name: "Decision",
                table: "Conclusion",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "AdministrativeInspectionInvestigationStage",
                columns: table => new
                {
                    AdministrativeInspectionsAdministrativeInspectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    InvestigationStagesInvestigationStageId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdministrativeInspectionInvestigationStage", x => new { x.AdministrativeInspectionsAdministrativeInspectionId, x.InvestigationStagesInvestigationStageId });
                    table.ForeignKey(
                        name: "FK_AdministrativeInspectionInvestigationStage_AdministrativeInspection_AdministrativeInspectionsAdministrativeInspectionId",
                        column: x => x.AdministrativeInspectionsAdministrativeInspectionId,
                        principalTable: "AdministrativeInspection",
                        principalColumn: "AdministrativeInspectionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdministrativeInspectionInvestigationStage_InvestigationStage_InvestigationStagesInvestigationStageId",
                        column: x => x.InvestigationStagesInvestigationStageId,
                        principalTable: "InvestigationStage",
                        principalColumn: "InvestigationStageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComplainInvestigationStage",
                columns: table => new
                {
                    ComplainsComplainId = table.Column<int>(type: "INTEGER", nullable: false),
                    StagesInvestigationStageId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplainInvestigationStage", x => new { x.ComplainsComplainId, x.StagesInvestigationStageId });
                    table.ForeignKey(
                        name: "FK_ComplainInvestigationStage_Complain_ComplainsComplainId",
                        column: x => x.ComplainsComplainId,
                        principalTable: "Complain",
                        principalColumn: "ComplainId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComplainInvestigationStage_InvestigationStage_StagesInvestigationStageId",
                        column: x => x.StagesInvestigationStageId,
                        principalTable: "InvestigationStage",
                        principalColumn: "InvestigationStageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestigationInvestigationStage",
                columns: table => new
                {
                    InvestigationsInvestigationId = table.Column<int>(type: "INTEGER", nullable: false),
                    StagesInvestigationStageId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationInvestigationStage", x => new { x.InvestigationsInvestigationId, x.StagesInvestigationStageId });
                    table.ForeignKey(
                        name: "FK_InvestigationInvestigationStage_InvestigationStage_StagesInvestigationStageId",
                        column: x => x.StagesInvestigationStageId,
                        principalTable: "InvestigationStage",
                        principalColumn: "InvestigationStageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestigationInvestigationStage_Investigation_InvestigationsInvestigationId",
                        column: x => x.InvestigationsInvestigationId,
                        principalTable: "Investigation",
                        principalColumn: "InvestigationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdministrativeInspectionInvestigationStage_InvestigationStagesInvestigationStageId",
                table: "AdministrativeInspectionInvestigationStage",
                column: "InvestigationStagesInvestigationStageId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplainInvestigationStage_StagesInvestigationStageId",
                table: "ComplainInvestigationStage",
                column: "StagesInvestigationStageId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationInvestigationStage_StagesInvestigationStageId",
                table: "InvestigationInvestigationStage",
                column: "StagesInvestigationStageId");
        }
    }
}
