using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UTPWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RebuildedDatabase1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investigator_LocalUser_LocalUserRef",
                table: "Investigator");

            migrationBuilder.DropIndex(
                name: "IX_Investigator_LocalUserRef",
                table: "Investigator");

            migrationBuilder.DropColumn(
                name: "InvestigatorId",
                table: "LocalUser");

            migrationBuilder.RenameColumn(
                name: "LocalUserRef",
                table: "Investigator",
                newName: "LocalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Investigator_LocalUserId",
                table: "Investigator",
                column: "LocalUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Investigator_LocalUser_LocalUserId",
                table: "Investigator",
                column: "LocalUserId",
                principalTable: "LocalUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investigator_LocalUser_LocalUserId",
                table: "Investigator");

            migrationBuilder.DropIndex(
                name: "IX_Investigator_LocalUserId",
                table: "Investigator");

            migrationBuilder.RenameColumn(
                name: "LocalUserId",
                table: "Investigator",
                newName: "LocalUserRef");

            migrationBuilder.AddColumn<int>(
                name: "InvestigatorId",
                table: "LocalUser",
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
    }
}
