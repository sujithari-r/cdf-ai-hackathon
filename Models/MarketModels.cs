namespace RenewableDashboard.Models;

public sealed record TrendPoint(string Month, decimal Price);

public sealed record MarketData(
    decimal ElectricityPrice,
    decimal CapacityGrowth,
    decimal RenewableShare,
    IReadOnlyList<TrendPoint> Trend);

public sealed class MarketSnapshot
{
    public int Id { get; set; }
    public string Period { get; set; } = string.Empty;
    public decimal ElectricityPrice { get; set; }
    public decimal CapacityGrowth { get; set; }
    public decimal RenewableShare { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
