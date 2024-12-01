using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrderPickingSystem.Migrations
{
    /// <inheritdoc />
    public partial class seeduserswithroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CurrentOrderId", "PasswordHash", "Username" },
                values: new object[,]
                {
                    { 1000, null, "$2a$11$RfwwHJPVyQY6iC.JVefNZ.QVU/4UEVrEYpLZuu3A6nbQCj8b.QUkS", "Vlad" },
                    { 1001, null, "$2a$11$gzPgmFLRvQS7UPHj.SyCGujRYji0EZqi2Mc1QuujtmiWUTIW0RNlG", "Alex" }
                });

            migrationBuilder.InsertData(
                table: "UserRoleMappings",
                columns: new[] { "Role", "UserId" },
                values: new object[,]
                {
                    { 1, 1000 },
                    { 2, 1000 },
                    { 1, 1001 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoleMappings",
                keyColumns: new[] { "Role", "UserId" },
                keyValues: new object[] { 1, 1000 });

            migrationBuilder.DeleteData(
                table: "UserRoleMappings",
                keyColumns: new[] { "Role", "UserId" },
                keyValues: new object[] { 2, 1000 });

            migrationBuilder.DeleteData(
                table: "UserRoleMappings",
                keyColumns: new[] { "Role", "UserId" },
                keyValues: new object[] { 1, 1001 });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1000);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1001);
        }
    }
}
