using Microsoft.EntityFrameworkCore;
using RenewableDashboard.Core.DTOs;
using RenewableDashboard.Core.Interfaces;
using RenewableDashboard.Infrastructure.Data;

namespace RenewableDashboard.Infrastructure.Services;

public class LocationDataService : ILocationService
{
    private readonly AppDbContext _context;

    public LocationDataService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<LocationMapDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Locations
            .OrderBy(l => l.Name)
            .Select(l => new LocationMapDto
            {
                Name = l.Name,
                Latitude = l.Latitude,
                Longitude = l.Longitude,
                ElectricityRate = l.ElectricityRate,
                SolarScore = l.SolarScore,
                Note = l.Note
            })
            .ToListAsync(cancellationToken);
    }
}
