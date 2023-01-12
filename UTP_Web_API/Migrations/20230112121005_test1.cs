using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UTPWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investigation_Company_CompanyId",
                table: "Investigation");

            migrationBuilder.DropForeignKey(
                name: "FK_Investigation_Conclusion_ConclusionId",
                table: "Investigation");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Investigation",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "ConclusionId",
                table: "Investigation",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Investigation_Conclusion_ConclusionId",
                table: "Investigation",
                column: "ConclusionId",
                principalTable: "Conclusion",
                principalColumn: "ConclusionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investigation_Company_CompanyId",
                table: "Investigation");

            migrationBuilder.DropForeignKey(
                name: "FK_Investigation_Conclusion_ConclusionId",
                table: "Investigation");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Investigation",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ConclusionId",
                table: "Investigation",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Investigation_Conclusion_ConclusionId",
                table: "Investigation",
                column: "ConclusionId",
                principalTable: "Conclusion",
                principalColumn: "ConclusionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
