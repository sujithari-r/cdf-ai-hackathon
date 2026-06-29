using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RenewableDashboard.Data;
using RenewableDashboard.Models;

namespace RenewableDashboard.Services;

/// <summary>
/// Provides market data. Attempts the live EIA Open Data API (when an API key is
/// configured) and otherwise falls back to the trend persisted in SQL.
/// Ported from the original /api/market route.
/// </summary>
public class MarketService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MarketService> _logger;

    public MarketService(
        IDbContextFactory<AppDbContext> dbFactory,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<MarketService> logger)
    {
        _dbFactory = dbFactory;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<MarketData> GetMarketDataAsync()
    {
        var apiKey = _configuration["EIA_API_KEY"]
            ?? Environment.GetEnvironmentVariable("EIA_API_KEY");

        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            var live = await TryFetchFromEiaAsync(apiKey);
            if (live is not null)
            {
                return live;
            }
        }

        return await GetFromDatabaseAsync();
    }

    private async Task<MarketData?> TryFetchFromEiaAsync(string apiKey)
    {
        try
        {
            var url =
                "https://api.eia.gov/v2/electricity/retail-sales/data/" +
                $"?api_key={apiKey}&data[0]=price&frequency=monthly" +
                "&facets[stateid][]=US&facets[sectorid][]=ALL&length=12" +
                "&sort[0][column]=period&sort[0][direction]=desc";

            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            using var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            var rows = doc.RootElement.GetProperty("response").GetProperty("data");

            var trend = new List<TrendPoint>();
            double latestPrice = 0;

            // Rows arrive newest-first; reverse to build an ascending trend.
            var items = rows.EnumerateArray().ToList();
            for (var i = items.Count - 1; i >= 0; i--)
            {
                var item = items[i];
                var month = item.GetProperty("period").GetString() ?? string.Empty;
                var price = ReadDouble(item, "price") / 100.0;
                trend.Add(new TrendPoint(month, price));
            }

            if (items.Count > 0)
            {
                latestPrice = ReadDouble(items[0], "price") / 100.0;
            }

            var summary = await GetSummaryAsync();

            return new MarketData
            {
                ElectricityPrice = latestPrice,
                CapacityGrowth = summary.CapacityGrowth,
                RenewableShare = summary.RenewableShare,
                Trend = trend,
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "EIA market fetch failed; falling back to SQL data.");
            return null;
        }
    }

    private static double ReadDouble(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var prop))
        {
            return 0;
        }

        return prop.ValueKind switch
        {
            JsonValueKind.Number => prop.GetDouble(),
            JsonValueKind.String when double.TryParse(prop.GetString(), out var v) => v,
            _ => 0,
        };
    }

    private async Task<(double CapacityGrowth, double RenewableShare)> GetSummaryAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var summary = await db.MarketSummaries.FirstOrDefaultAsync();
        return (summary?.CapacityGrowth ?? 0, summary?.RenewableShare ?? 0);
    }

    private async Task<MarketData> GetFromDatabaseAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        var trendPoints = await db.MarketTrendPoints
            .OrderBy(p => p.Sequence)
            .Select(p => new TrendPoint(p.Month, p.Price))
            .ToListAsync();

        var summary = await db.MarketSummaries.FirstOrDefaultAsync();

        return new MarketData
        {
            ElectricityPrice = trendPoints.Count > 0 ? trendPoints[^1].Price : 0,
            CapacityGrowth = summary?.CapacityGrowth ?? 0,
            RenewableShare = summary?.RenewableShare ?? 0,
            Trend = trendPoints,
        };
    }

    /// <summary>Computes summary statistics over a trend window (ported from market page).</summary>
    public static TrendSummary? BuildTrendSummary(IReadOnlyList<TrendPoint> trend)
    {
        if (trend.Count < 2)
        {
            return null;
        }

        var first = trend[0].Price;
        var last = trend[^1].Price;
        var change = last - first;
        var percentage = first != 0 ? (change / first) * 100 : 0;

        return new TrendSummary
        {
            First = first,
            Last = last,
            Change = change,
            Percentage = percentage,
            IsPositive = change >= 0,
            LatestMonth = trend[^1].Month,
            Average = trend.Average(p => p.Price),
            Min = trend.Min(p => p.Price),
            Max = trend.Max(p => p.Price),
        };
    }
}
