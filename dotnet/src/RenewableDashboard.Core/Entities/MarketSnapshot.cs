namespace RenewableDashboard.Core.Entities;

public class MarketSnapshot
{
    public int Id { get; set; }
    public double ElectricityPrice { get; set; }
    public double CapacityGrowth { get; set; }
    public double RenewableShare { get; set; }
    public DateTime FetchedAt { get; set; } = DateTime.UtcNow;
    public ICollection<MarketTrend> Trends { get; set; } = new List<MarketTrend>();
}
