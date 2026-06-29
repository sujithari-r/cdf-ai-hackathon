using Microsoft.EntityFrameworkCore;
using RenewableDashboard.Core.Models;

namespace RenewableDashboard.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Location> Locations => Set<Location>();
    public DbSet<MarketIndicator> MarketIndicators => Set<MarketIndicator>();
    public DbSet<MarketSnapshot> MarketSnapshots => Set<MarketSnapshot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasIndex(l => l.Name).IsUnique();
            entity.Property(l => l.ElectricityRate).HasPrecision(10, 4);
            entity.Property(l => l.SolarScore).HasPrecision(4, 2);
        });

        modelBuilder.Entity<MarketIndicator>(entity =>
        {
            entity.HasIndex(m => m.Key).IsUnique();
            entity.Property(m => m.Value).HasPrecision(10, 4);
        });

        modelBuilder.Entity<MarketSnapshot>(entity =>
        {
            entity.Property(m => m.ElectricityPrice).HasPrecision(10, 4);
            entity.Property(m => m.CapacityGrowth).HasPrecision(10, 4);
            entity.Property(m => m.RenewableShare).HasPrecision(10, 4);
        });
    }
}
