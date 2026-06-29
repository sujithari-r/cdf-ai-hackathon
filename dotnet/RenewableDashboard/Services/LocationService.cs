using Microsoft.EntityFrameworkCore;
using RenewableDashboard.Data;
using RenewableDashboard.Data.Entities;

namespace RenewableDashboard.Services;

/// <summary>Reads tracked locations from SQL.</summary>
public class LocationService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public LocationService(IDbContextFactory<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<List<Location>> GetLocationsAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Locations.OrderBy(l => l.Name).ToListAsync();
    }
}
