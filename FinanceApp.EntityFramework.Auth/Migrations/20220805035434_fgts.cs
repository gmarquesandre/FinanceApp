using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.EntityFramework.User.Migrations
{
    public partial class fgts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MonthAniversaryWithdraw",
                table: "FGTS",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5c4999d9-c354-4e79-8423-ccf29ea553e1", "AQAAAAEAACcQAAAAEF8s0E6b9MuRDWV3gtm7SJOtfFepUc7yGzzLEznwJZq/uursoHbMdcAnLFvp3hWd6Q==", "d5ad7857-3ef7-4f8c-a591-3d8509fb58a1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthAniversaryWithdraw",
                table: "FGTS");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d8d4fad8-4c95-448c-a6e8-ee9cc6356b96", "AQAAAAEAACcQAAAAEHwIz2q2/nfcGLnWvFJaVfRGUDpFIRiJ4VVj3W7astw3VYImFrstVYSb0ClWKGRjZA==", "f7c79f12-fe93-4678-8dc0-92da71a5775b" });
        }
    }
}
