using Microsoft.EntityFrameworkCore;
using RenewableDashboard.Core.Entities;

namespace RenewableDashboard.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Location> Locations => Set<Location>();
    public DbSet<MarketSnapshot> MarketSnapshots => Set<MarketSnapshot>();
    public DbSet<MarketTrend> MarketTrends => Set<MarketTrend>();
    public DbSet<ProjectScenario> ProjectScenarios => Set<ProjectScenario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Location>(e =>
        {
            e.HasKey(l => l.Id);
            e.Property(l => l.Name).HasMaxLength(200).IsRequired();
            e.Property(l => l.ElectricityRate).HasPrecision(10, 6);
            e.Property(l => l.Note).HasMaxLength(500);
        });

        modelBuilder.Entity<MarketSnapshot>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.ElectricityPrice).HasPrecision(10, 6);
            e.HasMany(s => s.Trends).WithOne(t => t.MarketSnapshot).HasForeignKey(t => t.MarketSnapshotId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MarketTrend>(e =>
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.Month).HasMaxLength(20).IsRequired();
            e.Property(t => t.Price).HasPrecision(10, 6);
        });

        modelBuilder.Entity<ProjectScenario>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Name).HasMaxLength(200);
            e.Property(s => s.ScenarioType).HasMaxLength(50);
            e.Property(s => s.RateMode).HasMaxLength(50);
        });

        // Seed initial location data
        modelBuilder.Entity<Location>().HasData(
            new Location { Id = 1, Name = "Texas", Latitude = 31.0, Longitude = -100.0, ElectricityRate = 0.12, SolarScore = 85, Note = "Strong solar potential, deregulated market" },
            new Location { Id = 2, Name = "California", Latitude = 36.7, Longitude = -119.4, ElectricityRate = 0.23, SolarScore = 92, Note = "Highest rates, excellent incentives" },
            new Location { Id = 3, Name = "Arizona", Latitude = 34.0, Longitude = -111.0, ElectricityRate = 0.13, SolarScore = 95, Note = "Peak sun hours, growing market" }
        );
    }
}
