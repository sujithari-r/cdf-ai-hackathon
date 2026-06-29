using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RenewableDashboard.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalculationSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Scenario = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    RateMode = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    ManualElectricityRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    ActiveElectricityRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalProjectCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    AnnualRevenue = table.Column<decimal>(type: "TEXT", nullable: false),
                    NetOperatingIncome = table.Column<decimal>(type: "TEXT", nullable: false),
                    PaybackPeriodYears = table.Column<decimal>(type: "TEXT", nullable: true),
                    Npv = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculationSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocationInsights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    ElectricityRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    SolarScore = table.Column<decimal>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Latitude = table.Column<decimal>(type: "TEXT", nullable: false),
                    Longitude = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationInsights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarketSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ElectricityPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    CapacityGrowth = table.Column<decimal>(type: "TEXT", nullable: false),
                    RenewableShare = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarketTrendPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MarketSnapshotId = table.Column<int>(type: "INTEGER", nullable: false),
                    Month = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketTrendPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketTrendPoints_MarketSnapshots_MarketSnapshotId",
                        column: x => x.MarketSnapshotId,
                        principalTable: "MarketSnapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationInsights_Name",
                table: "LocationInsights",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MarketTrendPoints_MarketSnapshotId",
                table: "MarketTrendPoints",
                column: "MarketSnapshotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalculationSnapshots");

            migrationBuilder.DropTable(
                name: "LocationInsights");

            migrationBuilder.DropTable(
                name: "MarketTrendPoints");

            migrationBuilder.DropTable(
                name: "MarketSnapshots");
        }
    }
}
