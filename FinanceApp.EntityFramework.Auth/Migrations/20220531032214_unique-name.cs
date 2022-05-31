using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.EntityFramework.Migrations
{
    public partial class uniquename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 99997,
                column: "ConcurrencyStamp",
                value: "8e2f2c6f-1607-44d9-b2d1-8eb3c5b11a9e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 99999,
                column: "ConcurrencyStamp",
                value: "d810e98b-9cf7-4670-82cd-f2da60f52882");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 99999,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f62dc90b-39bb-4861-a799-69a4936ae327", "AQAAAAEAACcQAAAAEHLsCOkkXZdVzDTwDJlnMXmqNv5v0Y5VJfh55/bAQyhWVQlc4W8LbDqscIxVIavJPg==", "9d4b88e7-5a88-4d72-af15-2de489c0fcea" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 99997,
                column: "ConcurrencyStamp",
                value: "cdad4c39-e9a3-44d6-ab14-f9bdd82c4001");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 99999,
                column: "ConcurrencyStamp",
                value: "cc7097e8-a895-4dd0-a00c-a02e0fd758bf");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 99999,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2b5573b9-8171-4f7c-bb14-04ad6e53f002", "AQAAAAEAACcQAAAAEFNCETHev0c8MGfPDx6zBE663jaWf9w+N/NdvZWDZNTclTbeYdojDKBGYzKFgnF39w==", "821158b3-3f08-4036-9e34-d907e67a0b33" });
        }
    }
}
