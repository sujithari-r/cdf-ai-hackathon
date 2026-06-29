using Microsoft.EntityFrameworkCore;
using RenewableDashboard.Models;

namespace RenewableDashboard.Data;

public sealed class DashboardDbContext(DbContextOptions<DashboardDbContext> options) : DbContext(options)
{
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<MarketSnapshot> MarketSnapshots => Set<MarketSnapshot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(location => location.Id);
            entity.Property(location => location.Name).HasMaxLength(120).IsRequired();
            entity.Property(location => location.ElectricityRate).HasPrecision(10, 4);
            entity.Property(location => location.Note).HasMaxLength(500).IsRequired();
            entity.HasIndex(location => location.Name).IsUnique();

            entity.HasData(
                new Location
                {
                    Id = 1,
                    Name = "Texas",
                    Latitude = 31.0,
                    Longitude = -99.0,
                    ElectricityRate = 0.14m,
                    SolarScore = 8.5,
                    Note = "Strong wind and solar development potential"
                },
                new Location
                {
                    Id = 2,
                    Name = "California",
                    Latitude = 36.7,
                    Longitude = -119.4,
                    ElectricityRate = 0.22m,
                    SolarScore = 9.2,
                    Note = "High electricity prices and strong solar market"
                },
                new Location
                {
                    Id = 3,
                    Name = "Arizona",
                    Latitude = 34.2,
                    Longitude = -111.7,
                    ElectricityRate = 0.13m,
                    SolarScore = 9.5,
                    Note = "Excellent solar resource availability"
                });
        });

        modelBuilder.Entity<MarketSnapshot>(entity =>
        {
            entity.HasKey(snapshot => snapshot.Id);
            entity.Property(snapshot => snapshot.Period).HasMaxLength(24).IsRequired();
            entity.Property(snapshot => snapshot.ElectricityPrice).HasPrecision(10, 4);
            entity.Property(snapshot => snapshot.CapacityGrowth).HasPrecision(10, 2);
            entity.Property(snapshot => snapshot.RenewableShare).HasPrecision(10, 2);
            entity.HasIndex(snapshot => snapshot.Period);
        });
    }
}
