using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RenewableDashboard.Core.DTOs;
using RenewableDashboard.Core.Entities;
using RenewableDashboard.Core.Interfaces;
using RenewableDashboard.Infrastructure.Data;

namespace RenewableDashboard.Infrastructure.Services;

public class EiaMarketService : IMarketService
{
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EiaMarketService> _logger;

    public EiaMarketService(HttpClient httpClient, AppDbContext context, IConfiguration configuration, ILogger<EiaMarketService> logger)
    {
        _httpClient = httpClient;
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<MarketDataDto> GetMarketDataAsync()
    {
        // Return cached data if fetched within the last hour
        var recent = await _context.MarketSnapshots
            .Include(s => s.Trends)
            .OrderByDescending(s => s.FetchedAt)
            .FirstOrDefaultAsync();

        if (recent != null && (DateTime.UtcNow - recent.FetchedAt).TotalHours < 1)
        {
            return MapToDto(recent);
        }

        return await FetchFromEiaAsync();
    }

    private async Task<MarketDataDto> FetchFromEiaAsync()
    {
        try
        {
            var apiKey = _configuration["EIA_API_KEY"] ?? string.Empty;
            var url = $"https://api.eia.gov/v2/electricity/retail-sales/data/?api_key={apiKey}&frequency=monthly&data[0]=price&facets[stateid][]=US&facets[sectorid][]=ALL&sort[0][column]=period&sort[0][direction]=desc&length=12";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var responseData = doc.RootElement.GetProperty("response").GetProperty("data");
            var prices = new List<(string month, double price)>();

            foreach (var item in responseData.EnumerateArray())
            {
                var period = item.GetProperty("period").GetString() ?? "";
                var priceStr = item.GetProperty("price").GetString() ?? "0";
                if (double.TryParse(priceStr, out double priceCentsPerKwh))
                {
                    prices.Add((period, priceCentsPerKwh / 100.0));
                }
            }

            prices.Reverse();

            var snapshot = new MarketSnapshot
            {
                ElectricityPrice = prices.LastOrDefault().price,
                CapacityGrowth = 8.5,
                RenewableShare = 32,
                FetchedAt = DateTime.UtcNow,
                Trends = prices.Select(p => new MarketTrend { Month = p.month, Price = p.price }).ToList()
            };

            _context.MarketSnapshots.Add(snapshot);
            await _context.SaveChangesAsync();

            return MapToDto(snapshot);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch EIA market data");
            return new MarketDataDto
            {
                ElectricityPrice = 0,
                CapacityGrowth = 8.5,
                RenewableShare = 32,
                Trend = new List<TrendPointDto>()
            };
        }
    }

    private static MarketDataDto MapToDto(MarketSnapshot snapshot) => new()
    {
        ElectricityPrice = snapshot.ElectricityPrice,
        CapacityGrowth = snapshot.CapacityGrowth,
        RenewableShare = snapshot.RenewableShare,
        Trend = snapshot.Trends
            .OrderBy(t => t.Month)
            .Select(t => new TrendPointDto { Month = t.Month, Price = t.Price })
            .ToList()
    };
}
