using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UTPWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedNullableInvestigatorToComplains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complain_Investigator_InvestigatorId",
                table: "Complain");

            migrationBuilder.AlterColumn<int>(
                name: "InvestigatorId",
                table: "Complain",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Complain_Investigator_InvestigatorId",
                table: "Complain",
                column: "InvestigatorId",
                principalTable: "Investigator",
                principalColumn: "InvestigatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complain_Investigator_InvestigatorId",
                table: "Complain");

            migrationBuilder.AlterColumn<int>(
                name: "InvestigatorId",
                table: "Complain",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Complain_Investigator_InvestigatorId",
                table: "Complain",
                column: "InvestigatorId",
                principalTable: "Investigator",
                principalColumn: "InvestigatorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
