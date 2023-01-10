using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UTPWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixingRelationshipsBetweenLocalUserAndInvestigator1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocalUser_Investigator_InvestigatorRef",
                table: "LocalUser");

            migrationBuilder.DropIndex(
                name: "IX_LocalUser_InvestigatorRef",
                table: "LocalUser");

            migrationBuilder.DropColumn(
                name: "InvestigatorRef",
                table: "LocalUser");

            migrationBuilder.AddColumn<int>(
                name: "LocalUserRef",
                table: "Investigator",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Investigator_LocalUserRef",
                table: "Investigator",
                column: "LocalUserRef",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Investigator_LocalUser_LocalUserRef",
                table: "Investigator",
                column: "LocalUserRef",
                principalTable: "LocalUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investigator_LocalUser_LocalUserRef",
                table: "Investigator");

            migrationBuilder.DropIndex(
                name: "IX_Investigator_LocalUserRef",
                table: "Investigator");

            migrationBuilder.DropColumn(
                name: "LocalUserRef",
                table: "Investigator");

            migrationBuilder.AddColumn<int>(
                name: "InvestigatorRef",
                table: "LocalUser",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LocalUser_InvestigatorRef",
                table: "LocalUser",
                column: "InvestigatorRef",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LocalUser_Investigator_InvestigatorRef",
                table: "LocalUser",
                column: "InvestigatorRef",
                principalTable: "Investigator",
                principalColumn: "InvestigatorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
