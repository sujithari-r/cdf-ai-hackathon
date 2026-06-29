using Microsoft.EntityFrameworkCore;
using RenewableDashboard.Web.Models;

namespace RenewableDashboard.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<LocationInsight> LocationInsights => Set<LocationInsight>();
    public DbSet<CalculationSnapshot> CalculationSnapshots => Set<CalculationSnapshot>();
    public DbSet<MarketSnapshot> MarketSnapshots => Set<MarketSnapshot>();
    public DbSet<MarketTrendPoint> MarketTrendPoints => Set<MarketTrendPoint>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LocationInsight>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(120);
            entity.Property(x => x.Note).HasMaxLength(500);
            entity.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<CalculationSnapshot>(entity =>
        {
            entity.Property(x => x.Scenario).HasMaxLength(32);
            entity.Property(x => x.RateMode).HasMaxLength(32);
        });

        modelBuilder.Entity<MarketSnapshot>(entity =>
        {
            entity.HasMany(x => x.Trend)
                .WithOne(x => x.MarketSnapshot)
                .HasForeignKey(x => x.MarketSnapshotId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MarketTrendPoint>(entity =>
        {
            entity.Property(x => x.Month).HasMaxLength(16);
        });
    }
}
