using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UTPWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangedInvestiationRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investigation_Company_CompanyId",
                table: "Investigation");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Investigation",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Investigation_Company_CompanyId",
                table: "Investigation",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investigation_Company_CompanyId",
                table: "Investigation");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Investigation",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Investigation_Company_CompanyId",
                table: "Investigation",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyId");
        }
    }
}
