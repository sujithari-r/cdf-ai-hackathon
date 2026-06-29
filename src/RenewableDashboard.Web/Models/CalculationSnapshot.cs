namespace RenewableDashboard.Web.Models;

public class CalculationSnapshot
{
    public int Id { get; set; }
    public string Scenario { get; set; } = "base";
    public string RateMode { get; set; } = "manual";
    public decimal ManualElectricityRate { get; set; }
    public decimal ActiveElectricityRate { get; set; }
    public decimal TotalProjectCost { get; set; }
    public decimal AnnualRevenue { get; set; }
    public decimal NetOperatingIncome { get; set; }
    public decimal? PaybackPeriodYears { get; set; }
    public decimal Npv { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}
