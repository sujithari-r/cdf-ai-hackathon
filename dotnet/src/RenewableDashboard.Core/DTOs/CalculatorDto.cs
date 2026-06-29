namespace RenewableDashboard.Core.DTOs;

public class CalculatorInputDto
{
    public double SystemSizeKw { get; set; } = 100;
    public double CapacityFactor { get; set; } = 25;
    public double InstallCostPerW { get; set; } = 1.20;
    public double ElectricityRate { get; set; } = 0.12;
    public double OmCost { get; set; } = 5000;
    public string Scenario { get; set; } = "base";
    public string RateMode { get; set; } = "manual";
    public double? MapElectricityRate { get; set; }
}

public class CalculatorResultDto
{
    public double TotalProjectCost { get; set; }
    public double AnnualEnergyProduction { get; set; }
    public double AnnualRevenue { get; set; }
    public double NetOperatingIncome { get; set; }
    public double? PaybackPeriod { get; set; }
    public double Npv { get; set; }
    public double ActiveElectricityRate { get; set; }
    public double ActiveCapacityFactor { get; set; }
    public double ActiveOmCost { get; set; }
    public List<CashFlowPoint> CashFlows { get; set; } = new();

    public CalculatorResultDto? CompareResult { get; set; }
}

public class CashFlowPoint
{
    public int Year { get; set; }
    public double Amount { get; set; }
}
