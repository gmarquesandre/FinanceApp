using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.EntityFramework.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    AssetCodeISIN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssetCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: false),
                    DateLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.AssetCodeISIN);
                });

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndexLastValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndexName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndexLastValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndexValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndexName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndexValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentFundValueHistoric",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CNPJ = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentFundValueHistoric", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentFundValues",
                columns: table => new
                {
                    CNPJ = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameShort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Situation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: false),
                    TaxLongTerm = table.Column<bool>(type: "bit", nullable: false),
                    FundTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdministrationFee = table.Column<double>(type: "float", nullable: false),
                    AdministrationFeeInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PerformanceFee = table.Column<double>(type: "float", nullable: false),
                    PerformanceFeeInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BenchmarkIndex = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentFundValues", x => x.CNPJ);
                });

            migrationBuilder.CreateTable(
                name: "ProspectIndexValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndexName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateResearch = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Median = table.Column<double>(type: "float", nullable: false),
                    Average = table.Column<double>(type: "float", nullable: false),
                    Min = table.Column<double>(type: "float", nullable: false),
                    Max = table.Column<double>(type: "float", nullable: false),
                    ResearchAnswers = table.Column<int>(type: "int", nullable: false),
                    BaseCalculo = table.Column<int>(type: "int", nullable: false),
                    DateLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProspectIndexValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TreasuryBondValueHistoric",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeISIN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TreasuryBondName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FixedInterestValueBuy = table.Column<double>(type: "float", nullable: false),
                    FixedInterestValueSell = table.Column<double>(type: "float", nullable: false),
                    UnitPriceBuy = table.Column<double>(type: "float", nullable: false),
                    UnitPriceSell = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreasuryBondValueHistoric", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TreasuryBondValues",
                columns: table => new
                {
                    CodeISIN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TreasuryBondName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IndexName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FixedInterestValueBuy = table.Column<double>(type: "float", nullable: false),
                    FixedInterestValueSell = table.Column<double>(type: "float", nullable: false),
                    DateLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnitPriceBuy = table.Column<double>(type: "float", nullable: false),
                    UnitPriceSell = table.Column<double>(type: "float", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastAvailableDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreasuryBondValues", x => x.CodeISIN);
                });

            migrationBuilder.CreateTable(
                name: "WorkingDaysByYear",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    WorkingDays = table.Column<int>(type: "int", nullable: false),
                    DateLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingDaysByYear", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetChanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetCodeISIN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeclarationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GroupingFactor = table.Column<double>(type: "float", nullable: false),
                    ToAssetISIN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetChanges_Assets_AssetCodeISIN",
                        column: x => x.AssetCodeISIN,
                        principalTable: "Assets",
                        principalColumn: "AssetCodeISIN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetEarnings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetCodeISIN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeclarationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CashAmount = table.Column<double>(type: "float", nullable: false),
                    Period = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetEarnings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetEarnings_Assets_AssetCodeISIN",
                        column: x => x.AssetCodeISIN,
                        principalTable: "Assets",
                        principalColumn: "AssetCodeISIN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetChanges_AssetCodeISIN",
                table: "AssetChanges",
                column: "AssetCodeISIN");

            migrationBuilder.CreateIndex(
                name: "IX_AssetChanges_Hash",
                table: "AssetChanges",
                column: "Hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetEarnings_AssetCodeISIN",
                table: "AssetEarnings",
                column: "AssetCodeISIN");

            migrationBuilder.CreateIndex(
                name: "IX_AssetEarnings_Hash",
                table: "AssetEarnings",
                column: "Hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetCodeISIN",
                table: "Assets",
                column: "AssetCodeISIN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Holidays_CountryCode_Date",
                table: "Holidays",
                columns: new[] { "CountryCode", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndexLastValues_IndexName",
                table: "IndexLastValues",
                column: "IndexName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndexValues_Date_IndexName",
                table: "IndexValues",
                columns: new[] { "Date", "IndexName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentFundValueHistoric_Date_CNPJ",
                table: "InvestmentFundValueHistoric",
                columns: new[] { "Date", "CNPJ" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryBondValueHistoric_CodeISIN_Date",
                table: "TreasuryBondValueHistoric",
                columns: new[] { "CodeISIN", "Date" },
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
                name: "AssetChanges");

            migrationBuilder.DropTable(
                name: "AssetEarnings");

            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DropTable(
                name: "IndexLastValues");

            migrationBuilder.DropTable(
                name: "IndexValues");

            migrationBuilder.DropTable(
                name: "InvestmentFundValueHistoric");

            migrationBuilder.DropTable(
                name: "InvestmentFundValues");

            migrationBuilder.DropTable(
                name: "ProspectIndexValues");

            migrationBuilder.DropTable(
                name: "TreasuryBondValueHistoric");

            migrationBuilder.DropTable(
                name: "TreasuryBondValues");

            migrationBuilder.DropTable(
                name: "WorkingDaysByYear");

            migrationBuilder.DropTable(
                name: "Assets");
        }
    }
}
