using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.EntityFramework.Auth.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AdditionalFixedInterest",
                table: "PrivateFixedIncomes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateTime",
                table: "PrivateFixedIncomes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateTime",
                table: "PrivateFixedIncomes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 99997,
                column: "ConcurrencyStamp",
                value: "34f27f66-5443-40f7-90cd-c42701e954a0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 99999,
                column: "ConcurrencyStamp",
                value: "0276d971-a4b8-4378-a30b-2b0f8486f3e2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 99999,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2e779958-9647-4376-befa-3862ca1828b8", "AQAAAAEAACcQAAAAEOut9cZU1B/ukJPcZFNFXC7iC1LQ3jItJT7XRecayqMUUEomCEsyW4i2MHV1wYwCwg==", "99d2a5e6-b183-4c54-9701-ba16dd105a49" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDateTime",
                table: "PrivateFixedIncomes");

            migrationBuilder.DropColumn(
                name: "UpdateDateTime",
                table: "PrivateFixedIncomes");

            migrationBuilder.AlterColumn<decimal>(
                name: "AdditionalFixedInterest",
                table: "PrivateFixedIncomes",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 99997,
                column: "ConcurrencyStamp",
                value: "0f864938-8d77-42f7-9a6c-4d441d9dbb65");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 99999,
                column: "ConcurrencyStamp",
                value: "542ffeaf-d033-4048-894f-b52124699162");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 99999,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e9f24604-f2d8-4b48-bf3c-15bd4b5cc419", "AQAAAAEAACcQAAAAEEKDrSdBJvXsL+La+rVwNtf16RHXnGM9ZZYSlx1Tcz8AuJT0K/gftECHpOF9oEJjkQ==", "f810af26-56e6-4baf-b4b3-dbe97126753d" });
        }
    }
}
