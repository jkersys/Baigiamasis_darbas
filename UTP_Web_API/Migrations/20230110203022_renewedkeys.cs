using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UTPWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class renewedkeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investigator_LocalUser_LocalUserId",
                table: "Investigator");

            migrationBuilder.DropIndex(
                name: "IX_Investigator_LocalUserId",
                table: "Investigator");

            migrationBuilder.DropColumn(
                name: "LocalUserId",
                table: "Investigator");

            migrationBuilder.AddColumn<int>(
                name: "InvestigatorId",
                table: "LocalUser",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Decision",
                table: "Conclusion",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_LocalUser_InvestigatorId",
                table: "LocalUser",
                column: "InvestigatorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LocalUser_Investigator_InvestigatorId",
                table: "LocalUser",
                column: "InvestigatorId",
                principalTable: "Investigator",
                principalColumn: "InvestigatorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocalUser_Investigator_InvestigatorId",
                table: "LocalUser");

            migrationBuilder.DropIndex(
                name: "IX_LocalUser_InvestigatorId",
                table: "LocalUser");

            migrationBuilder.DropColumn(
                name: "InvestigatorId",
                table: "LocalUser");

            migrationBuilder.AddColumn<int>(
                name: "LocalUserId",
                table: "Investigator",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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
    }
}
