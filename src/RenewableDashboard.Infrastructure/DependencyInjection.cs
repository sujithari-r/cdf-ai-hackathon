using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RenewableDashboard.Infrastructure.Data;

namespace RenewableDashboard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString,
        bool useSqlite = false)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            if (useSqlite)
            {
                options.UseSqlite(connectionString);
            }
            else
            {
                options.UseSqlServer(connectionString);
            }
        });

        services.AddScoped<Core.Interfaces.ILocationService, Services.LocationDataService>();
        services.AddScoped<Core.Interfaces.IMarketService, Services.EiaMarketService>();
        services.AddScoped<Core.Interfaces.IAssistantService, Services.OpenAiAssistantService>();

        return services;
    }

    public static async Task InitializeDatabaseAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await DbSeeder.SeedAsync(context);
    }
}
