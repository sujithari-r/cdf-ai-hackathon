using Microsoft.EntityFrameworkCore;
using RenewableDashboard.Data.Entities;

namespace RenewableDashboard.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Location> Locations => Set<Location>();
    public DbSet<MarketTrendPoint> MarketTrendPoints => Set<MarketTrendPoint>();
    public DbSet<MarketSummary> MarketSummaries => Set<MarketSummary>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<CalculatorRun> CalculatorRuns => Set<CalculatorRun>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seeded locations (ported from the original lib/locationData.ts).
        modelBuilder.Entity<Location>().HasData(
            new Location
            {
                Id = 1,
                Name = "Texas",
                Latitude = 31.0,
                Longitude = -99.0,
                ElectricityRate = 0.14,
                SolarScore = 8.5,
                Note = "Strong wind and solar development potential",
            },
            new Location
            {
                Id = 2,
                Name = "California",
                Latitude = 36.7,
                Longitude = -119.4,
                ElectricityRate = 0.22,
                SolarScore = 9.2,
                Note = "High electricity prices and strong solar market",
            },
            new Location
            {
                Id = 3,
                Name = "Arizona",
                Latitude = 34.2,
                Longitude = -111.7,
                ElectricityRate = 0.13,
                SolarScore = 9.5,
                Note = "Excellent solar resource availability",
            });

        // Seeded market indicators (matching the original API defaults).
        modelBuilder.Entity<MarketSummary>().HasData(
            new MarketSummary { Id = 1, CapacityGrowth = 8.5, RenewableShare = 32 });

        // Seeded national monthly electricity price trend ($/kWh) used as an
        // offline fallback when the live EIA API is unavailable.
        var trend = new (string Month, double Price)[]
        {
            ("2024-01", 0.165),
            ("2024-02", 0.166),
            ("2024-03", 0.168),
            ("2024-04", 0.170),
            ("2024-05", 0.173),
            ("2024-06", 0.178),
            ("2024-07", 0.181),
            ("2024-08", 0.182),
            ("2024-09", 0.179),
            ("2024-10", 0.176),
            ("2024-11", 0.174),
            ("2024-12", 0.175),
        };

        var points = new List<MarketTrendPoint>();
        for (var i = 0; i < trend.Length; i++)
        {
            points.Add(new MarketTrendPoint
            {
                Id = i + 1,
                Month = trend[i].Month,
                Price = trend[i].Price,
                Sequence = i,
            });
        }

        modelBuilder.Entity<MarketTrendPoint>().HasData(points);
    }
}
