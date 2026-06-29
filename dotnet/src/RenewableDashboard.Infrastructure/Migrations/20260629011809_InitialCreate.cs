using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RenewableDashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    ElectricityRate = table.Column<double>(type: "float(10)", precision: 10, scale: 6, nullable: false),
                    SolarScore = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarketSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElectricityPrice = table.Column<double>(type: "float(10)", precision: 10, scale: 6, nullable: false),
                    CapacityGrowth = table.Column<double>(type: "float", nullable: false),
                    RenewableShare = table.Column<double>(type: "float", nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectScenarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ScenarioType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RateMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SystemSizeKw = table.Column<double>(type: "float", nullable: false),
                    CapacityFactor = table.Column<double>(type: "float", nullable: false),
                    InstallCostPerW = table.Column<double>(type: "float", nullable: false),
                    ManualElectricityRate = table.Column<double>(type: "float", nullable: false),
                    ActiveElectricityRate = table.Column<double>(type: "float", nullable: false),
                    OmCost = table.Column<double>(type: "float", nullable: false),
                    TotalProjectCost = table.Column<double>(type: "float", nullable: false),
                    AnnualEnergyProduction = table.Column<double>(type: "float", nullable: false),
                    AnnualRevenue = table.Column<double>(type: "float", nullable: false),
                    NetOperatingIncome = table.Column<double>(type: "float", nullable: false),
                    PaybackPeriod = table.Column<double>(type: "float", nullable: true),
                    Npv = table.Column<double>(type: "float", nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectScenarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarketTrends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarketSnapshotId = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Price = table.Column<double>(type: "float(10)", precision: 10, scale: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketTrends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketTrends_MarketSnapshots_MarketSnapshotId",
                        column: x => x.MarketSnapshotId,
                        principalTable: "MarketSnapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "CreatedAt", "ElectricityRate", "Latitude", "Longitude", "Name", "Note", "SolarScore" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 29, 1, 18, 8, 996, DateTimeKind.Utc).AddTicks(3727), 0.12, 31.0, -100.0, "Texas", "Strong solar potential, deregulated market", 85 },
                    { 2, new DateTime(2026, 6, 29, 1, 18, 8, 996, DateTimeKind.Utc).AddTicks(3738), 0.23000000000000001, 36.700000000000003, -119.40000000000001, "California", "Highest rates, excellent incentives", 92 },
                    { 3, new DateTime(2026, 6, 29, 1, 18, 8, 996, DateTimeKind.Utc).AddTicks(3740), 0.13, 34.0, -111.0, "Arizona", "Peak sun hours, growing market", 95 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarketTrends_MarketSnapshotId",
                table: "MarketTrends",
                column: "MarketSnapshotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "MarketTrends");

            migrationBuilder.DropTable(
                name: "ProjectScenarios");

            migrationBuilder.DropTable(
                name: "MarketSnapshots");
        }
    }
}
