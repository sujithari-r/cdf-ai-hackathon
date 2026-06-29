namespace RenewableDashboard.Models;

public record TrendPoint(string Month, double Price);

public class MarketData
{
    public double ElectricityPrice { get; set; }
    public double CapacityGrowth { get; set; }
    public double RenewableShare { get; set; }
    public List<TrendPoint> Trend { get; set; } = new();
}

public class TrendSummary
{
    public double First { get; set; }
    public double Last { get; set; }
    public double Change { get; set; }
    public double Percentage { get; set; }
    public bool IsPositive { get; set; }
    public string LatestMonth { get; set; } = "--";
    public double Average { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
}
