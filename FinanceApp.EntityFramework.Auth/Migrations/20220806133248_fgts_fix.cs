using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.EntityFramework.User.Migrations
{
    public partial class fgts_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentValue",
                table: "FGTS",
                newName: "CurrentBalance");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6c94ea29-0f3b-4169-84ae-433663ca27a0", "AQAAAAEAACcQAAAAEMBkSO5B6dHRowVQjGtYSeC67aMifKXjEHb19xzS3/Nkj6wOh5vDsGAeoC34GPupPw==", "83d5e9b4-e813-449e-b3a2-899d76c77ccc" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentBalance",
                table: "FGTS",
                newName: "CurrentValue");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5c4999d9-c354-4e79-8423-ccf29ea553e1", "AQAAAAEAACcQAAAAEF8s0E6b9MuRDWV3gtm7SJOtfFepUc7yGzzLEznwJZq/uursoHbMdcAnLFvp3hWd6Q==", "d5ad7857-3ef7-4f8c-a591-3d8509fb58a1" });
        }
    }
}
