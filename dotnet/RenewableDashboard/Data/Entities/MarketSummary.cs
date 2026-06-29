namespace RenewableDashboard.Data.Entities;

/// <summary>
/// Singleton-style market indicators that accompany the price trend.
/// </summary>
public class MarketSummary
{
    public int Id { get; set; }

    /// <summary>Year-over-year renewable capacity growth indicator (%).</summary>
    public double CapacityGrowth { get; set; }

    /// <summary>Renewable share of generation (%).</summary>
    public double RenewableShare { get; set; }
}
