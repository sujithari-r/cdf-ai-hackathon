namespace RenewableDashboard.Web.Models;

public class CalculatorInput
{
    public decimal SystemSizeKw { get; set; } = 5000;
    public decimal CapacityFactorPercent { get; set; } = 25;
    public decimal InstallCostPerWatt { get; set; } = 1.2m;
    public decimal ElectricityRate { get; set; } = 0.12m;
    public decimal AnnualOmCost { get; set; } = 50000;
    public string Scenario { get; set; } = "base";
    public string RateMode { get; set; } = "manual";
}
