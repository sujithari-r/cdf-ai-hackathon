using RenewableDashboard.Core.Entities;

namespace RenewableDashboard.Core.Interfaces;

public interface ILocationRepository
{
    Task<IEnumerable<Location>> GetAllAsync();
    Task<Location?> GetByIdAsync(int id);
    Task<Location> AddAsync(Location location);
    Task UpdateAsync(Location location);
    Task DeleteAsync(int id);
}
