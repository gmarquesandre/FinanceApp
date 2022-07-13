using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.Api.Migrations
{
    public partial class teste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDateTime",
                table: "CurrentBalances",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "CurrentBalances",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fdf65da2-0f81-4795-8659-b890ce5b046b", "AQAAAAEAACcQAAAAEM62b8mN2aDCjQaSVObg2gpm57AkEcBvV8Ayyv4+BMRxhOkwiGHQnuxxCa/RYJm5Og==", "b151c36b-0b94-4d8f-8c94-c8ee2a2b5f00" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDateTime",
                table: "CurrentBalances",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDateTime",
                table: "CurrentBalances",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ff63be55-dd8d-46e8-8bf7-791cb0aa847d", "AQAAAAEAACcQAAAAEI4joUumZvTQS6V7vapRnYdCu69cuiSUuMkdYuMXPMeQre85aamVo9Z6T4ET6ldnJA==", "02f5ed54-2452-4599-9369-c394020f935f" });
        }
    }
}
