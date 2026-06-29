using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RenewableDashboard.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalculatorRuns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Scenario = table.Column<string>(type: "TEXT", nullable: false),
                    RateMode = table.Column<string>(type: "TEXT", nullable: false),
                    SystemSizeKw = table.Column<double>(type: "REAL", nullable: false),
                    CapacityFactor = table.Column<double>(type: "REAL", nullable: false),
                    InstallCostPerW = table.Column<double>(type: "REAL", nullable: false),
                    ElectricityRate = table.Column<double>(type: "REAL", nullable: false),
                    AnnualOMCost = table.Column<double>(type: "REAL", nullable: false),
                    TotalProjectCost = table.Column<double>(type: "REAL", nullable: false),
                    AnnualRevenue = table.Column<double>(type: "REAL", nullable: false),
                    NetOperatingIncome = table.Column<double>(type: "REAL", nullable: false),
                    PaybackPeriod = table.Column<double>(type: "REAL", nullable: true),
                    Npv = table.Column<double>(type: "REAL", nullable: false),
                    LocationName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculatorRuns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionId = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    ElectricityRate = table.Column<double>(type: "REAL", nullable: false),
                    SolarScore = table.Column<double>(type: "REAL", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarketSummaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CapacityGrowth = table.Column<double>(type: "REAL", nullable: false),
                    RenewableShare = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketSummaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarketTrendPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Month = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    Sequence = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketTrendPoints", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "ElectricityRate", "Latitude", "Longitude", "Name", "Note", "SolarScore" },
                values: new object[,]
                {
                    { 1, 0.14000000000000001, 31.0, -99.0, "Texas", "Strong wind and solar development potential", 8.5 },
                    { 2, 0.22, 36.700000000000003, -119.40000000000001, "California", "High electricity prices and strong solar market", 9.1999999999999993 },
                    { 3, 0.13, 34.200000000000003, -111.7, "Arizona", "Excellent solar resource availability", 9.5 }
                });

            migrationBuilder.InsertData(
                table: "MarketSummaries",
                columns: new[] { "Id", "CapacityGrowth", "RenewableShare" },
                values: new object[] { 1, 8.5, 32.0 });

            migrationBuilder.InsertData(
                table: "MarketTrendPoints",
                columns: new[] { "Id", "Month", "Price", "Sequence" },
                values: new object[,]
                {
                    { 1, "2024-01", 0.16500000000000001, 0 },
                    { 2, "2024-02", 0.16600000000000001, 1 },
                    { 3, "2024-03", 0.16800000000000001, 2 },
                    { 4, "2024-04", 0.17000000000000001, 3 },
                    { 5, "2024-05", 0.17299999999999999, 4 },
                    { 6, "2024-06", 0.17799999999999999, 5 },
                    { 7, "2024-07", 0.18099999999999999, 6 },
                    { 8, "2024-08", 0.182, 7 },
                    { 9, "2024-09", 0.17899999999999999, 8 },
                    { 10, "2024-10", 0.17599999999999999, 9 },
                    { 11, "2024-11", 0.17399999999999999, 10 },
                    { 12, "2024-12", 0.17499999999999999, 11 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalculatorRuns");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "MarketSummaries");

            migrationBuilder.DropTable(
                name: "MarketTrendPoints");
        }
    }
}
