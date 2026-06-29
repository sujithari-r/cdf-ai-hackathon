namespace RenewableDashboard.Core.Entities;

public class ProjectScenario
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ScenarioType { get; set; } = "base";
    public string RateMode { get; set; } = "manual";
    public double SystemSizeKw { get; set; }
    public double CapacityFactor { get; set; }
    public double InstallCostPerW { get; set; }
    public double ManualElectricityRate { get; set; }
    public double ActiveElectricityRate { get; set; }
    public double OmCost { get; set; }
    public double TotalProjectCost { get; set; }
    public double AnnualEnergyProduction { get; set; }
    public double AnnualRevenue { get; set; }
    public double NetOperatingIncome { get; set; }
    public double? PaybackPeriod { get; set; }
    public double Npv { get; set; }
    public string? LocationName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
