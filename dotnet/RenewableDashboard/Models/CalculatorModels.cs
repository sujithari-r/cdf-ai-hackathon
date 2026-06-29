namespace RenewableDashboard.Models;

public enum Scenario
{
    Base,
    Optimistic,
}

public enum RateMode
{
    Manual,
    Map,
    Compare,
}

/// <summary>Editable project assumptions for the economics calculator.</summary>
public class CalculatorInputs
{
    public Scenario Scenario { get; set; } = Scenario.Base;
    public RateMode RateMode { get; set; } = RateMode.Manual;

    public double SystemSizeKw { get; set; } = 5000;
    public double CapacityFactor { get; set; } = 25;
    public double InstallCostPerW { get; set; } = 1.2;
    public double ElectricityRate { get; set; } = 0.12;
    public double AnnualOMCost { get; set; } = 50000;
}

/// <summary>Computed financial outputs for a single rate scenario.</summary>
public class CalculatorMetrics
{
    public double TotalProjectCost { get; set; }
    public double AnnualEnergyProduction { get; set; }
    public double AnnualRevenue { get; set; }
    public double NetOperatingIncome { get; set; }
    public double? PaybackPeriod { get; set; }
    public double Npv { get; set; }
}

/// <summary>A snapshot of the active calculator state shared across the dashboard.</summary>
public class CalculatorSnapshot
{
    public Scenario Scenario { get; set; }
    public RateMode RateMode { get; set; }
    public double ManualElectricityRate { get; set; }
    public double ActiveElectricityRate { get; set; }
    public double TotalProjectCost { get; set; }
    public double AnnualRevenue { get; set; }
    public double NetOperatingIncome { get; set; }
    public double? PaybackPeriod { get; set; }
    public double Npv { get; set; }
}

public record CashFlowPoint(string Year, double CashFlow);
