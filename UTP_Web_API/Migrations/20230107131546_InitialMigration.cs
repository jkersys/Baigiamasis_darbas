using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UTPWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyRegistrationNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyAdress = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyEmail = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyPhone = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "Conclusion",
                columns: table => new
                {
                    ConclusionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Decision = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conclusion", x => x.ConclusionId);
                });

            migrationBuilder.CreateTable(
                name: "InvestigationStage",
                columns: table => new
                {
                    InvestigationStageId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Stage = table.Column<string>(type: "TEXT", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationStage", x => x.InvestigationStageId);
                });

            migrationBuilder.CreateTable(
                name: "LocalUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "BLOB", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdministrativeInspection",
                columns: table => new
                {
                    AdministrativeInspectionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    ConclusionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdministrativeInspection", x => x.AdministrativeInspectionId);
                    table.ForeignKey(
                        name: "FK_AdministrativeInspection_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdministrativeInspection_Conclusion_ConclusionId",
                        column: x => x.ConclusionId,
                        principalTable: "Conclusion",
                        principalColumn: "ConclusionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Investigation",
                columns: table => new
                {
                    InvestigationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    LegalBase = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ConclusionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Penalty = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investigation", x => x.InvestigationId);
                    table.ForeignKey(
                        name: "FK_Investigation_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Investigation_Conclusion_ConclusionId",
                        column: x => x.ConclusionId,
                        principalTable: "Conclusion",
                        principalColumn: "ConclusionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Investigator",
                columns: table => new
                {
                    InvestigatorId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CertificationId = table.Column<string>(type: "TEXT", nullable: false),
                    CabinetNumber = table.Column<string>(type: "TEXT", nullable: false),
                    WorkAdress = table.Column<string>(type: "TEXT", nullable: false),
                    LocalUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investigator", x => x.InvestigatorId);
                    table.ForeignKey(
                        name: "FK_Investigator_LocalUser_LocalUserId",
                        column: x => x.LocalUserId,
                        principalTable: "LocalUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "AdministrativeInspectionInvestigator",
                columns: table => new
                {
                    AdministrativeInspectionsAdministrativeInspectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    InvestigatorsInvestigatorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdministrativeInspectionInvestigator", x => new { x.AdministrativeInspectionsAdministrativeInspectionId, x.InvestigatorsInvestigatorId });
                    table.ForeignKey(
                        name: "FK_AdministrativeInspectionInvestigator_AdministrativeInspection_AdministrativeInspectionsAdministrativeInspectionId",
                        column: x => x.AdministrativeInspectionsAdministrativeInspectionId,
                        principalTable: "AdministrativeInspection",
                        principalColumn: "AdministrativeInspectionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdministrativeInspectionInvestigator_Investigator_InvestigatorsInvestigatorId",
                        column: x => x.InvestigatorsInvestigatorId,
                        principalTable: "Investigator",
                        principalColumn: "InvestigatorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Complain",
                columns: table => new
                {
                    ComplainId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyInformation = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ConclusionId = table.Column<int>(type: "INTEGER", nullable: false),
                    LocalUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    InvestigatorId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complain", x => x.ComplainId);
                    table.ForeignKey(
                        name: "FK_Complain_Conclusion_ConclusionId",
                        column: x => x.ConclusionId,
                        principalTable: "Conclusion",
                        principalColumn: "ConclusionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Complain_Investigator_InvestigatorId",
                        column: x => x.InvestigatorId,
                        principalTable: "Investigator",
                        principalColumn: "InvestigatorId");
                    table.ForeignKey(
                        name: "FK_Complain_LocalUser_LocalUserId",
                        column: x => x.LocalUserId,
                        principalTable: "LocalUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestigationInvestigator",
                columns: table => new
                {
                    InvestigationsInvestigationId = table.Column<int>(type: "INTEGER", nullable: false),
                    InvestigatorsInvestigatorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationInvestigator", x => new { x.InvestigationsInvestigationId, x.InvestigatorsInvestigatorId });
                    table.ForeignKey(
                        name: "FK_InvestigationInvestigator_Investigation_InvestigationsInvestigationId",
                        column: x => x.InvestigationsInvestigationId,
                        principalTable: "Investigation",
                        principalColumn: "InvestigationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestigationInvestigator_Investigator_InvestigatorsInvestigatorId",
                        column: x => x.InvestigatorsInvestigatorId,
                        principalTable: "Investigator",
                        principalColumn: "InvestigatorId",
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

            migrationBuilder.InsertData(
                table: "Conclusion",
                columns: new[] { "ConclusionId", "Decision" },
                values: new object[,]
                {
                    { 1, "Skundas atmestas" },
                    { 2, "Skundas priimtas" },
                    { 3, "Pažeidimų nenustatyta" },
                    { 4, "Nutraukta dėl mažareikšmiškumo" },
                    { 5, "Nustatyti pažeidimai, byla perduota komisijai" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdministrativeInspection_CompanyId",
                table: "AdministrativeInspection",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AdministrativeInspection_ConclusionId",
                table: "AdministrativeInspection",
                column: "ConclusionId");

            migrationBuilder.CreateIndex(
                name: "IX_AdministrativeInspectionInvestigationStage_InvestigationStagesInvestigationStageId",
                table: "AdministrativeInspectionInvestigationStage",
                column: "InvestigationStagesInvestigationStageId");

            migrationBuilder.CreateIndex(
                name: "IX_AdministrativeInspectionInvestigator_InvestigatorsInvestigatorId",
                table: "AdministrativeInspectionInvestigator",
                column: "InvestigatorsInvestigatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Complain_ConclusionId",
                table: "Complain",
                column: "ConclusionId");

            migrationBuilder.CreateIndex(
                name: "IX_Complain_InvestigatorId",
                table: "Complain",
                column: "InvestigatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Complain_LocalUserId",
                table: "Complain",
                column: "LocalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplainInvestigationStage_StagesInvestigationStageId",
                table: "ComplainInvestigationStage",
                column: "StagesInvestigationStageId");

            migrationBuilder.CreateIndex(
                name: "IX_Investigation_CompanyId",
                table: "Investigation",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Investigation_ConclusionId",
                table: "Investigation",
                column: "ConclusionId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationInvestigationStage_StagesInvestigationStageId",
                table: "InvestigationInvestigationStage",
                column: "StagesInvestigationStageId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationInvestigator_InvestigatorsInvestigatorId",
                table: "InvestigationInvestigator",
                column: "InvestigatorsInvestigatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Investigator_LocalUserId",
                table: "Investigator",
                column: "LocalUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdministrativeInspectionInvestigationStage");

            migrationBuilder.DropTable(
                name: "AdministrativeInspectionInvestigator");

            migrationBuilder.DropTable(
                name: "ComplainInvestigationStage");

            migrationBuilder.DropTable(
                name: "InvestigationInvestigationStage");

            migrationBuilder.DropTable(
                name: "InvestigationInvestigator");

            migrationBuilder.DropTable(
                name: "AdministrativeInspection");

            migrationBuilder.DropTable(
                name: "Complain");

            migrationBuilder.DropTable(
                name: "InvestigationStage");

            migrationBuilder.DropTable(
                name: "Investigation");

            migrationBuilder.DropTable(
                name: "Investigator");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Conclusion");

            migrationBuilder.DropTable(
                name: "LocalUser");
        }
    }
}
