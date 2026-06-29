namespace RenewableDashboard.Web.Models;

public class CalculatorResult
{
    public decimal ActiveElectricityRate { get; set; }
    public decimal TotalProjectCost { get; set; }
    public decimal AnnualEnergyProduction { get; set; }
    public decimal AnnualRevenue { get; set; }
    public decimal NetOperatingIncome { get; set; }
    public decimal? PaybackPeriodYears { get; set; }
    public decimal Npv { get; set; }
    public List<CashFlowPoint> CashFlow { get; set; } = [];
}

public class CashFlowPoint
{
    public int Year { get; set; }
    public decimal Value { get; set; }
}
