using Microsoft.EntityFrameworkCore;
using RenewableDashboard.Data;
using RenewableDashboard.Models;

namespace RenewableDashboard.Services;

public sealed class LocationService(DashboardDbContext dbContext)
{
    public Task<List<Location>> GetLocationsAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.Locations
            .AsNoTracking()
            .OrderBy(location => location.Name)
            .ToListAsync(cancellationToken);
    }
}
