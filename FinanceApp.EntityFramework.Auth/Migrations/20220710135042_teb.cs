using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.Api.Migrations
{
    public partial class teb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3732a0e6-318f-4cc3-ad4f-5aa5a60c18b0", "AQAAAAEAACcQAAAAEMtskipkDHA9QuVV/xyVecCHTdCHEzkXh9haR3siwzx4wA26lXZ2Qa3Yevuw9Bhjeg==", "c2721024-6eb6-45ad-9c48-ffdc53519387" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fdf65da2-0f81-4795-8659-b890ce5b046b", "AQAAAAEAACcQAAAAEM62b8mN2aDCjQaSVObg2gpm57AkEcBvV8Ayyv4+BMRxhOkwiGHQnuxxCa/RYJm5Og==", "b151c36b-0b94-4d8f-8c94-c8ee2a2b5f00" });
        }
    }
}
