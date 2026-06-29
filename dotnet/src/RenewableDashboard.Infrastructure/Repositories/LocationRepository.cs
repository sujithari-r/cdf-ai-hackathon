using Microsoft.EntityFrameworkCore;
using RenewableDashboard.Core.Entities;
using RenewableDashboard.Core.Interfaces;
using RenewableDashboard.Infrastructure.Data;

namespace RenewableDashboard.Infrastructure.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly AppDbContext _context;

    public LocationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Location>> GetAllAsync() =>
        await _context.Locations.OrderBy(l => l.Name).ToListAsync();

    public async Task<Location?> GetByIdAsync(int id) =>
        await _context.Locations.FindAsync(id);

    public async Task<Location> AddAsync(Location location)
    {
        _context.Locations.Add(location);
        await _context.SaveChangesAsync();
        return location;
    }

    public async Task UpdateAsync(Location location)
    {
        _context.Locations.Update(location);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var location = await _context.Locations.FindAsync(id);
        if (location != null)
        {
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
        }
    }
}
