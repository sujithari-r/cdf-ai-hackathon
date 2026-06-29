namespace RenewableDashboard.Core.DTOs;

public class TrendPointDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class MarketDataDto
{
    public decimal ElectricityPrice { get; set; }
    public decimal CapacityGrowth { get; set; }
    public decimal RenewableShare { get; set; }
    public List<TrendPointDto> Trend { get; set; } = [];
}
