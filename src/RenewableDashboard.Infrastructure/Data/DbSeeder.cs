using Microsoft.EntityFrameworkCore;
using RenewableDashboard.Core.Models;

namespace RenewableDashboard.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (!await context.Locations.AnyAsync())
        {
            context.Locations.AddRange(
                new Location
                {
                    Name = "Texas",
                    Latitude = 31.0,
                    Longitude = -99.0,
                    ElectricityRate = 0.14m,
                    SolarScore = 8.5m,
                    Note = "Strong wind and solar development potential"
                },
                new Location
                {
                    Name = "California",
                    Latitude = 36.7,
                    Longitude = -119.4,
                    ElectricityRate = 0.22m,
                    SolarScore = 9.2m,
                    Note = "High electricity prices and strong solar market"
                },
                new Location
                {
                    Name = "Arizona",
                    Latitude = 34.2,
                    Longitude = -111.7,
                    ElectricityRate = 0.13m,
                    SolarScore = 9.5m,
                    Note = "Excellent solar resource availability"
                });

            await context.SaveChangesAsync();
        }

        if (!await context.MarketIndicators.AnyAsync())
        {
            context.MarketIndicators.AddRange(
                new MarketIndicator
                {
                    Key = "CapacityGrowth",
                    Value = 8.5m,
                    Description = "Market growth indicator"
                },
                new MarketIndicator
                {
                    Key = "RenewableShare",
                    Value = 32m,
                    Description = "Renewable penetration"
                });

            await context.SaveChangesAsync();
        }
    }
}
