namespace RenewableDashboard.Models;

public sealed class CalculatorInputs
{
    public string Scenario { get; set; } = "base";
    public string RateMode { get; set; } = "manual";
    public double SystemSizeKw { get; set; } = 5000;
    public double CapacityFactor { get; set; } = 25;
    public decimal InstallCostPerW { get; set; } = 1.2m;
    public decimal ElectricityRate { get; set; } = 0.12m;
    public decimal AnnualOperationsMaintenanceCost { get; set; } = 50000m;
}

public sealed record CalculatorMetrics(
    decimal TotalProjectCost,
    double AnnualEnergyProduction,
    decimal AnnualRevenue,
    decimal NetOperatingIncome,
    double? PaybackPeriod,
    decimal NetPresentValue);

public sealed record CalculatorSnapshot(
    string Scenario,
    string RateMode,
    decimal ManualElectricityRate,
    decimal ActiveElectricityRate,
    decimal TotalProjectCost,
    decimal AnnualRevenue,
    decimal NetOperatingIncome,
    double? PaybackPeriod,
    decimal NetPresentValue);
