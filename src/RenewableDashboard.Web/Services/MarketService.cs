using System.Globalization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RenewableDashboard.Web.Data;
using RenewableDashboard.Web.Models;

namespace RenewableDashboard.Web.Services;

public class MarketService(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration,
    AppDbContext dbContext)
{
    public async Task<MarketSnapshot> GetLatestMarketSnapshotAsync(CancellationToken cancellationToken = default)
    {
        var apiKey = configuration["EiaApiKey"] ?? configuration["EIA_API_KEY"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return await GetFallbackSnapshotAsync(cancellationToken);
        }

        try
        {
            var client = httpClientFactory.CreateClient(nameof(MarketService));
            var url =
                $"https://api.eia.gov/v2/electricity/retail-sales/data/?api_key={apiKey}" +
                "&data[0]=price&frequency=monthly&facets[stateid][]=US&facets[sectorid][]=ALL&length=12" +
                "&sort[0][column]=period&sort[0][direction]=desc";

            using var response = await client.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

            if (!document.RootElement.TryGetProperty("response", out var responseNode) ||
                !responseNode.TryGetProperty("data", out var dataNode) ||
                dataNode.ValueKind != JsonValueKind.Array ||
                dataNode.GetArrayLength() == 0)
            {
                return await GetFallbackSnapshotAsync(cancellationToken);
            }

            var rows = dataNode.EnumerateArray().ToList();
            var latest = rows[0];
            var latestPrice = ParsePrice(latest);

            var trend = rows
                .AsEnumerable()
                .Reverse()
                .Select(item => new MarketTrendPoint
                {
                    Month = item.TryGetProperty("period", out var period)
                        ? period.GetString() ?? "unknown"
                        : "unknown",
                    Price = ParsePrice(item)
                })
                .ToList();

            var snapshot = new MarketSnapshot
            {
                ElectricityPrice = latestPrice,
                CapacityGrowth = 8.5m,
                RenewableShare = 32m,
                Trend = trend
            };

            dbContext.MarketSnapshots.Add(snapshot);
            await dbContext.SaveChangesAsync(cancellationToken);

            return snapshot;
        }
        catch
        {
            return await GetFallbackSnapshotAsync(cancellationToken);
        }
    }

    private async Task<MarketSnapshot> GetFallbackSnapshotAsync(CancellationToken cancellationToken)
    {
        var fallback = await dbContext.MarketSnapshots
            .Include(x => x.Trend)
            .OrderByDescending(x => x.CreatedUtc)
            .FirstOrDefaultAsync(cancellationToken);

        if (fallback is not null)
        {
            return fallback;
        }

        var months = new[] { "2025-07", "2025-08", "2025-09", "2025-10", "2025-11", "2025-12", "2026-01", "2026-02", "2026-03", "2026-04", "2026-05", "2026-06" };
        var baseline = new[] { 0.133m, 0.132m, 0.134m, 0.137m, 0.136m, 0.138m, 0.139m, 0.141m, 0.142m, 0.143m, 0.144m, 0.145m };

        return new MarketSnapshot
        {
            ElectricityPrice = baseline[^1],
            CapacityGrowth = 8.5m,
            RenewableShare = 32m,
            Trend = months
                .Select((month, index) => new MarketTrendPoint
                {
                    Month = month,
                    Price = baseline[index]
                })
                .ToList()
        };
    }

    private static decimal ParsePrice(JsonElement item)
    {
        if (!item.TryGetProperty("price", out var priceNode))
        {
            return 0m;
        }

        var raw = priceNode.ValueKind switch
        {
            JsonValueKind.String => priceNode.GetString(),
            JsonValueKind.Number => priceNode.GetRawText(),
            _ => null
        };

        if (string.IsNullOrWhiteSpace(raw))
        {
            return 0m;
        }

        return decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed)
            ? parsed / 100m
            : 0m;
    }
}
