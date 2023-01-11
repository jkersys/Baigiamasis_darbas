using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UTPWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RebuildedDatabase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Investigator_LocalUserId",
                table: "Investigator");

            migrationBuilder.CreateIndex(
                name: "IX_Investigator_LocalUserId",
                table: "Investigator",
                column: "LocalUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Investigator_LocalUserId",
                table: "Investigator");

            migrationBuilder.CreateIndex(
                name: "IX_Investigator_LocalUserId",
                table: "Investigator",
                column: "LocalUserId");
        }
    }
}
