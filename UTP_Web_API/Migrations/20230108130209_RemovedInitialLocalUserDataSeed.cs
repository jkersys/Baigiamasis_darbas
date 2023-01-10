using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UTPWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemovedInitialLocalUserDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocalUser",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Password",
                table: "LocalUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "LocalUser",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "LocalUser",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "PasswordHash", "PasswordSalt", "PhoneNumber", "Role" },
                values: new object[] { 1, "admin@admin.lt", "Admin", "Admin", "admin", new byte[0], new byte[0], 866666666L, 4 });
        }
    }
}
