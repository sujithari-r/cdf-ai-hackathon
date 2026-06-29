namespace RenewableDashboard.Web.Models;

public class MarketSnapshot
{
    public int Id { get; set; }
    public decimal ElectricityPrice { get; set; }
    public decimal CapacityGrowth { get; set; }
    public decimal RenewableShare { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public List<MarketTrendPoint> Trend { get; set; } = [];
}
