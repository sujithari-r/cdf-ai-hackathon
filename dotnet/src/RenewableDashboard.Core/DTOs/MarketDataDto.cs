namespace RenewableDashboard.Core.DTOs;

public class MarketDataDto
{
    public double ElectricityPrice { get; set; }
    public double CapacityGrowth { get; set; }
    public double RenewableShare { get; set; }
    public List<TrendPointDto> Trend { get; set; } = new();
}

public class TrendPointDto
{
    public string Month { get; set; } = string.Empty;
    public double Price { get; set; }
}
