using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.EntityFramework.Data.Migrations
{
    public partial class assetvalues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "Assets",
                newName: "TradeCount");

            migrationBuilder.AddColumn<double>(
                name: "AveragePrice",
                table: "Assets",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ClosingPrice",
                table: "Assets",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaxPrice",
                table: "Assets",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinPrice",
                table: "Assets",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OpeningPrice",
                table: "Assets",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "StockTradeCount",
                table: "Assets",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AveragePrice",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ClosingPrice",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "MaxPrice",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "MinPrice",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "OpeningPrice",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "StockTradeCount",
                table: "Assets");

            migrationBuilder.RenameColumn(
                name: "TradeCount",
                table: "Assets",
                newName: "UnitPrice");
        }
    }
}
