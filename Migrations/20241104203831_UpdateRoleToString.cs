using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderPickingSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoleToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRights",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "PasswordHash", "Role" },
                values: new object[] { "$2a$11$rN/k6FkqbVQTIxgOoRu48ukA1qbDt54BuFB4vXo6VhlBsuUmYSGOa", "Worker" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1001,
                columns: new[] { "PasswordHash", "Role" },
                values: new object[] { "$2a$11$mGWzCYPbV7FOnNhLEYnQyOdS5OSbKJO6ONH/g/GAQ8fHVWF2uprGi", "Worker" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1002,
                columns: new[] { "PasswordHash", "Role" },
                values: new object[] { "$2a$11$jsjx4kG9LDrCcHFGJoKel.3gGX1UgrXcLbzUrFJFVWsxDMPQDDyV2", "Worker" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1004,
                columns: new[] { "PasswordHash", "Role" },
                values: new object[] { "$2a$11$yDfd5g2csiwhuN6ySMyTDOPGv8Etz1TvppDgK5Vv5qNSCOnr3JmAi", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserRights",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "PasswordHash", "UserRights" },
                values: new object[] { "$2a$11$IP.C48VvB89qgPdZE/gRpO1hykJzkbh.LTgxeRbLPbDC8IO5TGt0m", 1 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1001,
                columns: new[] { "PasswordHash", "UserRights" },
                values: new object[] { "$2a$11$qRjLd6nYVhSqmbhffGj81ejMgoW7sHbsV7bnYSrAqSDEV84Z0eg26", 1 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1002,
                columns: new[] { "PasswordHash", "UserRights" },
                values: new object[] { "$2a$11$rALanMZH0dIIpX8KvHdJkeMp8e3N533BB4Mfj5MHn5RYoFrDj0HbS", 1 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1004,
                columns: new[] { "PasswordHash", "UserRights" },
                values: new object[] { "$2a$11$SGCsflodSDaRLr9zdTvLk.IZbL99RIsQ4UnkIvQVRBRSREpytJnEW", 0 });
        }
    }
}
