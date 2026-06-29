namespace RenewableDashboard.Core.Models;

public class MarketIndicator
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string? Description { get; set; }
}
