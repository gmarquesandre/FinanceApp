using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.EntityFramework.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Spendings",
                newName: "Recurrence");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Incomes",
                newName: "Recurrence");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Recurrence",
                table: "Spendings",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Recurrence",
                table: "Incomes",
                newName: "Type");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 99997,
                column: "ConcurrencyStamp",
                value: "e3255599-fc44-40a8-aea3-add7c06c9d04");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 99999,
                column: "ConcurrencyStamp",
                value: "7d5fce18-a54c-4721-9b2d-af8324dd62a3");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 99999,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dccccc53-2793-425b-a75a-7902128e15c6", "AQAAAAEAACcQAAAAELAVrnByvVFwPPRLhLeeu6GBDz0dMCEZc4aYjseo9WjuanjTRMFeDqwHW0jD8TW83Q==", "95e37262-ae8a-4a73-92a3-6663bed3fa5e" });
        }
    }
}
