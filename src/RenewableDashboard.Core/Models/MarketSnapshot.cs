namespace RenewableDashboard.Core.Models;

public class MarketSnapshot
{
    public int Id { get; set; }
    public decimal ElectricityPrice { get; set; }
    public decimal CapacityGrowth { get; set; }
    public decimal RenewableShare { get; set; }
    public string TrendJson { get; set; } = "[]";
    public DateTime FetchedAt { get; set; }
}
