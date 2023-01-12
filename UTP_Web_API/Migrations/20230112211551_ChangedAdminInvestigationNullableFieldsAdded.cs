using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UTPWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangedAdminInvestigationNullableFieldsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdministrativeInspection_Conclusion_ConclusionId",
                table: "AdministrativeInspection");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "AdministrativeInspection",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "AdministrativeInspection",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "ConclusionId",
                table: "AdministrativeInspection",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_AdministrativeInspection_Conclusion_ConclusionId",
                table: "AdministrativeInspection",
                column: "ConclusionId",
                principalTable: "Conclusion",
                principalColumn: "ConclusionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdministrativeInspection_Conclusion_ConclusionId",
                table: "AdministrativeInspection");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "AdministrativeInspection",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "AdministrativeInspection",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ConclusionId",
                table: "AdministrativeInspection",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AdministrativeInspection_Conclusion_ConclusionId",
                table: "AdministrativeInspection",
                column: "ConclusionId",
                principalTable: "Conclusion",
                principalColumn: "ConclusionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
