using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RenewableDashboard.Core.Interfaces;
using RenewableDashboard.Core.Services;
using RenewableDashboard.Infrastructure.Data;
using RenewableDashboard.Infrastructure.Repositories;
using RenewableDashboard.Infrastructure.Services;

namespace RenewableDashboard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Server=(localdb)\\mssqllocaldb;Database=RenewableDashboard;Trusted_Connection=True;";

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IProjectScenarioRepository, ProjectScenarioRepository>();

        services.AddHttpClient<IMarketService, EiaMarketService>();

        services.AddScoped<IAssistantService, OpenAiAssistantService>();
        services.AddScoped<CalculatorService>();

        return services;
    }

    public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
    }
}
