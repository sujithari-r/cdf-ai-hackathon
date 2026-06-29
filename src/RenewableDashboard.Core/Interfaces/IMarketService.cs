using RenewableDashboard.Core.DTOs;

namespace RenewableDashboard.Core.Interfaces;

public interface IMarketService
{
    Task<MarketDataDto> GetMarketDataAsync(CancellationToken cancellationToken = default);
}
