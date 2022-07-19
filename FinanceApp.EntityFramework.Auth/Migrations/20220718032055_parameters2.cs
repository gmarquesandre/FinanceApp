using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.Api.Migrations
{
    public partial class parameters2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForecastParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PercentageCdiLoan = table.Column<double>(type: "float", nullable: true),
                    PercentageCdiFixedInteresIncometSavings = table.Column<double>(type: "float", nullable: true),
                    PercentageCdiVariableIncome = table.Column<double>(type: "float", nullable: true),
                    SavingsLiquidPercentage = table.Column<double>(type: "float", nullable: true),
                    MonthsSavingWarning = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForecastParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForecastParameters_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "10cc373e-de50-4782-98be-64b86eec5104", "AQAAAAEAACcQAAAAEJk0VmDXv+gEvlmkC8ZJ+6EUQIkBJtH5/HGco65GVipR+L7j+abRk00EONIsP/AgIw==", "e257f1c3-6e2f-445d-bb41-9692893e3ca4" });

            migrationBuilder.CreateIndex(
                name: "IX_ForecastParameters_UserId",
                table: "ForecastParameters",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForecastParameters");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0f4362d5-b07c-4cd1-9a81-1a85c47f3c9d", "AQAAAAEAACcQAAAAEI9vyF2TTVDPqpBD8M/7kQgT2QxmESuU0nc40pDWjynpGfjV+nBMTzPnkJYUM2ndMg==", "2035d87d-8d42-438f-a36e-6dfb14597022" });
        }
    }
}
