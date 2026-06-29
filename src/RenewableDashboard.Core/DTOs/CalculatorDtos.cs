namespace RenewableDashboard.Core.DTOs;

public class CalculatorInputs
{
    public decimal SystemSizeKw { get; set; } = 5000;
    public decimal CapacityFactor { get; set; } = 25;
    public decimal InstallCostPerW { get; set; } = 1.2m;
    public decimal ElectricityRate { get; set; } = 0.12m;
    public decimal AnnualOMCost { get; set; } = 50000;
    public string Scenario { get; set; } = "base";
}

public class CalculatorMetrics
{
    public decimal TotalProjectCost { get; set; }
    public decimal AnnualEnergyProduction { get; set; }
    public decimal AnnualRevenue { get; set; }
    public decimal NetOperatingIncome { get; set; }
    public decimal? PaybackPeriod { get; set; }
    public decimal Npv { get; set; }
    public decimal ActiveElectricityRate { get; set; }
    public decimal OmCostForScenario { get; set; }
}

public class CashFlowPoint
{
    public string Year { get; set; } = string.Empty;
    public decimal CashFlow { get; set; }
}
