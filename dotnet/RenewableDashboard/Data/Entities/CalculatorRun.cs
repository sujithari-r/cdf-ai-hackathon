namespace RenewableDashboard.Data.Entities;

/// <summary>
/// A saved renewable project economics calculation, persisted to SQL for history.
/// </summary>
public class CalculatorRun
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Inputs
    public string Scenario { get; set; } = "base";
    public string RateMode { get; set; } = "manual";
    public double SystemSizeKw { get; set; }
    public double CapacityFactor { get; set; }
    public double InstallCostPerW { get; set; }
    public double ElectricityRate { get; set; }
    public double AnnualOMCost { get; set; }

    // Outputs
    public double TotalProjectCost { get; set; }
    public double AnnualRevenue { get; set; }
    public double NetOperatingIncome { get; set; }
    public double? PaybackPeriod { get; set; }
    public double Npv { get; set; }

    public string? LocationName { get; set; }
}
