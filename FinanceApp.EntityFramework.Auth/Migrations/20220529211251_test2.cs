using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.EntityFramework.Auth.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDateTime",
                table: "PrivateFixedIncomes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 99997,
                column: "ConcurrencyStamp",
                value: "b4b166e5-54e5-4ec3-917b-f83a68d1570f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 99999,
                column: "ConcurrencyStamp",
                value: "815ef27a-daf9-4249-9192-9384c37c9822");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 99999,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e5ec8d8f-58e5-4f70-96e4-f5a69d8ab078", "AQAAAAEAACcQAAAAEDnO+Dizi/VnbR8Nlop/9LeFMqNA9dNwQH2EA87DA7GuMPocDWk6WEWGT1Bd0JSGBg==", "3b3a63c5-5086-447f-bdd6-63f2ab0e22ff" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDateTime",
                table: "PrivateFixedIncomes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

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
    }
}
