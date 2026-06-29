namespace RenewableDashboard.Core.Entities;

public class MarketTrend
{
    public int Id { get; set; }
    public int MarketSnapshotId { get; set; }
    public MarketSnapshot MarketSnapshot { get; set; } = null!;
    public string Month { get; set; } = string.Empty;
    public double Price { get; set; }
}
