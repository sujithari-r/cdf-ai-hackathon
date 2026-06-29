namespace RenewableDashboard.Data.Entities;

/// <summary>
/// A single monthly national electricity price observation used to build the market trend.
/// </summary>
public class MarketTrendPoint
{
    public int Id { get; set; }

    /// <summary>Period label, e.g. "2024-01".</summary>
    public string Month { get; set; } = string.Empty;

    /// <summary>Price in $/kWh.</summary>
    public double Price { get; set; }

    /// <summary>Sort order, ascending in time.</summary>
    public int Sequence { get; set; }
}
