using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RenewableDashboard.Data;
using RenewableDashboard.Models;

namespace RenewableDashboard.Services;

public sealed class MarketService(HttpClient httpClient, IConfiguration configuration, DashboardDbContext dbContext, ILogger<MarketService> logger)
{
    private const decimal CapacityGrowth = 8.5m;
    private const decimal RenewableShare = 32m;

    public async Task<MarketData> GetMarketDataAsync(CancellationToken cancellationToken = default)
    {
        var apiKey = configuration["EIA_API_KEY"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            logger.LogWarning("EIA_API_KEY is not configured; returning empty market data.");
            return EmptyMarketData();
        }

        try
        {
            var requestUri = $"https://api.eia.gov/v2/electricity/retail-sales/data/?api_key={Uri.EscapeDataString(apiKey)}&data[0]=price&frequency=monthly&facets[stateid][]=US&facets[sectorid][]=ALL&length=12&sort[0][column]=period&sort[0][direction]=desc";
            await using var stream = await httpClient.GetStreamAsync(requestUri, cancellationToken);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
            var rows = document.RootElement.GetProperty("response").GetProperty("data")
                .EnumerateArray()
                .ToList();

            var trend = rows
                .AsEnumerable()
                .Reverse()
                .Select(item => new TrendPoint(
                    item.GetProperty("period").GetString() ?? string.Empty,
                    ReadDecimal(item, "price") / 100m))
                .ToList();

            var latest = rows.FirstOrDefault();
            var latestPrice = latest.ValueKind == JsonValueKind.Undefined
                ? 0m
                : ReadDecimal(latest, "price") / 100m;

            if (trend.Count > 0)
            {
                await CacheLatestSnapshotAsync(trend[^1].Month, latestPrice, cancellationToken);
            }

            return new MarketData(latestPrice, CapacityGrowth, RenewableShare, trend);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch market data from EIA.");
            return EmptyMarketData();
        }
    }

    private async Task CacheLatestSnapshotAsync(string period, decimal electricityPrice, CancellationToken cancellationToken)
    {
        var existing = await dbContext.MarketSnapshots
            .FirstOrDefaultAsync(snapshot => snapshot.Period == period, cancellationToken);

        if (existing is null)
        {
            dbContext.MarketSnapshots.Add(new MarketSnapshot
            {
                Period = period,
                ElectricityPrice = electricityPrice,
                CapacityGrowth = CapacityGrowth,
                RenewableShare = RenewableShare,
                CreatedAt = DateTimeOffset.UtcNow
            });
        }
        else
        {
            existing.ElectricityPrice = electricityPrice;
            existing.CapacityGrowth = CapacityGrowth;
            existing.RenewableShare = RenewableShare;
            existing.CreatedAt = DateTimeOffset.UtcNow;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static decimal ReadDecimal(JsonElement item, string propertyName)
    {
        var property = item.GetProperty(propertyName);

        return property.ValueKind switch
        {
            JsonValueKind.Number => property.GetDecimal(),
            JsonValueKind.String when decimal.TryParse(property.GetString(), out var value) => value,
            _ => 0m
        };
    }

    private static MarketData EmptyMarketData()
    {
        return new MarketData(0m, 0m, 0m, Array.Empty<TrendPoint>());
    }
}
