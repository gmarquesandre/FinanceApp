using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.EntityFramework.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssetCodeISIN = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AssetCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TypeAsset = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UnitPrice = table.Column<double>(type: "double", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateLastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "IndexValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Index = table.Column<int>(type: "int", nullable: false),
                    IndexRecurrence = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Value = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndexValues", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProspectIndexValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Index = table.Column<int>(type: "int", nullable: false),
                    IndexRecurrence = table.Column<int>(type: "int", nullable: false),
                    DateResearch = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateStart = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Median = table.Column<double>(type: "double", nullable: false),
                    Average = table.Column<double>(type: "double", nullable: false),
                    Min = table.Column<double>(type: "double", nullable: false),
                    Max = table.Column<double>(type: "double", nullable: false),
                    ResearchAnswers = table.Column<int>(type: "int", nullable: false),
                    BaseCalculo = table.Column<int>(type: "int", nullable: false),
                    DateLastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProspectIndexValues", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TreasuryBondTitle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    FixedInterestValueBuy = table.Column<double>(type: "double", nullable: false),
                    FixedInterestValueSell = table.Column<double>(type: "double", nullable: false),
                    UnitPriceBuy = table.Column<double>(type: "double", nullable: false),
                    UnitPriceSell = table.Column<double>(type: "double", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUpdateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreasuryBondTitle", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TreasuryBondValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    FixedInterestValueBuy = table.Column<double>(type: "double", nullable: false),
                    FixedInterestValueSell = table.Column<double>(type: "double", nullable: false),
                    UnitPriceBuy = table.Column<double>(type: "double", nullable: false),
                    UnitPriceSell = table.Column<double>(type: "double", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreasuryBondValues", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WorkingDaysByYear",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Year = table.Column<int>(type: "int", nullable: false),
                    WorkingDays = table.Column<int>(type: "int", nullable: false),
                    DateLastUpdate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingDaysByYear", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Holidays_Date",
                table: "Holidays",
                column: "Date",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndexValues_Date_DateEnd_Index",
                table: "IndexValues",
                columns: new[] { "Date", "DateEnd", "Index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProspectIndexValues_DateStart_DateEnd_Index_BaseCalculo",
                table: "ProspectIndexValues",
                columns: new[] { "DateStart", "DateEnd", "Index", "BaseCalculo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryBondTitle_ExpirationDate_Type",
                table: "TreasuryBondTitle",
                columns: new[] { "ExpirationDate", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryBondValues_Date_ExpirationDate_Type",
                table: "TreasuryBondValues",
                columns: new[] { "Date", "ExpirationDate", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkingDaysByYear_Year",
                table: "WorkingDaysByYear",
                column: "Year",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DropTable(
                name: "IndexValues");

            migrationBuilder.DropTable(
                name: "ProspectIndexValues");

            migrationBuilder.DropTable(
                name: "TreasuryBondTitle");

            migrationBuilder.DropTable(
                name: "TreasuryBondValues");

            migrationBuilder.DropTable(
                name: "WorkingDaysByYear");
        }
    }
}
