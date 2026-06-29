using Microsoft.EntityFrameworkCore;
using RenewableDashboard.Web.Models;

namespace RenewableDashboard.Web.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await dbContext.Database.MigrateAsync();

        if (await dbContext.LocationInsights.AnyAsync())
        {
            return;
        }

        dbContext.LocationInsights.AddRange(
            new LocationInsight
            {
                Name = "Texas",
                ElectricityRate = 0.14m,
                SolarScore = 8.5m,
                Note = "Strong wind and solar development potential",
                Latitude = 31.0m,
                Longitude = -99.0m
            },
            new LocationInsight
            {
                Name = "California",
                ElectricityRate = 0.22m,
                SolarScore = 9.2m,
                Note = "High electricity prices and strong solar market",
                Latitude = 36.7m,
                Longitude = -119.4m
            },
            new LocationInsight
            {
                Name = "Arizona",
                ElectricityRate = 0.13m,
                SolarScore = 9.5m,
                Note = "Excellent solar resource availability",
                Latitude = 34.2m,
                Longitude = -111.7m
            }
        );

        await dbContext.SaveChangesAsync();
    }
}
