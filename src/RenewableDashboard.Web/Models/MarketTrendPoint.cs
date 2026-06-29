namespace RenewableDashboard.Web.Models;

public class MarketTrendPoint
{
    public int Id { get; set; }
    public int MarketSnapshotId { get; set; }
    public MarketSnapshot? MarketSnapshot { get; set; }
    public string Month { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
