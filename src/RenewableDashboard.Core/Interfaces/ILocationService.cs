using RenewableDashboard.Core.DTOs;

namespace RenewableDashboard.Core.Interfaces;

public interface ILocationService
{
    Task<List<LocationMapDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
