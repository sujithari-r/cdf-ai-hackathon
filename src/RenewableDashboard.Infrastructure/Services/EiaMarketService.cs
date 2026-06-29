using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RenewableDashboard.Core.DTOs;
using RenewableDashboard.Core.Interfaces;
using RenewableDashboard.Core.Models;
using RenewableDashboard.Infrastructure.Data;

namespace RenewableDashboard.Infrastructure.Services;

public class EiaMarketService : IMarketService
{
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EiaMarketService> _logger;

    public EiaMarketService(
        HttpClient httpClient,
        AppDbContext context,
        IConfiguration configuration,
        ILogger<EiaMarketService> logger)
    {
        _httpClient = httpClient;
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<MarketDataDto> GetMarketDataAsync(CancellationToken cancellationToken = default)
    {
        var indicators = await _context.MarketIndicators
            .ToDictionaryAsync(i => i.Key, i => i.Value, cancellationToken);

        var capacityGrowth = indicators.GetValueOrDefault("CapacityGrowth", 8.5m);
        var renewableShare = indicators.GetValueOrDefault("RenewableShare", 32m);

        try
        {
            var apiKey = _configuration["EIA_API_KEY"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return await BuildFallbackAsync(capacityGrowth, renewableShare, cancellationToken);
            }

            var url =
                $"https://api.eia.gov/v2/electricity/retail-sales/data/?api_key={apiKey}&data[0]=price&frequency=monthly&facets[stateid][]=US&facets[sectorid][]=ALL&length=12&sort[0][column]=period&sort[0][direction]=desc";

            var response = await _httpClient.GetFromJsonAsync<EiaResponse>(url, cancellationToken);
            var rows = response?.Response?.Data ?? [];

            if (rows.Count == 0)
            {
                return await BuildFallbackAsync(capacityGrowth, renewableShare, cancellationToken);
            }

            var latest = rows[0];
            var trend = rows
                .AsEnumerable()
                .Reverse()
                .Select(item => new TrendPointDto
                {
                    Month = item.Period ?? string.Empty,
                    Price = (item.Price ?? 0) / 100m
                })
                .ToList();

            var marketData = new MarketDataDto
            {
                ElectricityPrice = (latest.Price ?? 0) / 100m,
                CapacityGrowth = capacityGrowth,
                RenewableShare = renewableShare,
                Trend = trend
            };

            await CacheSnapshotAsync(marketData, cancellationToken);
            return marketData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch EIA market data");
            return await BuildFallbackAsync(capacityGrowth, renewableShare, cancellationToken);
        }
    }

    private async Task<MarketDataDto> BuildFallbackAsync(
        decimal capacityGrowth,
        decimal renewableShare,
        CancellationToken cancellationToken)
    {
        var cached = await _context.MarketSnapshots
            .OrderByDescending(s => s.FetchedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (cached is not null)
        {
            var trend = JsonSerializer.Deserialize<List<TrendPointDto>>(cached.TrendJson) ?? [];
            return new MarketDataDto
            {
                ElectricityPrice = cached.ElectricityPrice,
                CapacityGrowth = cached.CapacityGrowth,
                RenewableShare = cached.RenewableShare,
                Trend = trend
            };
        }

        return new MarketDataDto
        {
            ElectricityPrice = 0,
            CapacityGrowth = capacityGrowth,
            RenewableShare = renewableShare,
            Trend = []
        };
    }

    private async Task CacheSnapshotAsync(MarketDataDto data, CancellationToken cancellationToken)
    {
        _context.MarketSnapshots.Add(new MarketSnapshot
        {
            ElectricityPrice = data.ElectricityPrice,
            CapacityGrowth = data.CapacityGrowth,
            RenewableShare = data.RenewableShare,
            TrendJson = JsonSerializer.Serialize(data.Trend),
            FetchedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(cancellationToken);
    }

    private sealed class EiaResponse
    {
        public EiaResponseBody? Response { get; set; }
    }

    private sealed class EiaResponseBody
    {
        public List<EiaDataRow>? Data { get; set; }
    }

    private sealed class EiaDataRow
    {
        public string? Period { get; set; }
        public decimal? Price { get; set; }
    }
}
