namespace RenewableDashboard.Core.DTOs;

public class SelectedLocationDto
{
    public string Name { get; set; } = string.Empty;
    public decimal ElectricityRate { get; set; }
    public decimal SolarScore { get; set; }
    public string Note { get; set; } = string.Empty;
}

public class CalculatorSnapshotDto
{
    public string Scenario { get; set; } = "base";
    public string RateMode { get; set; } = "manual";
    public decimal ManualElectricityRate { get; set; }
    public decimal ActiveElectricityRate { get; set; }
    public decimal TotalProjectCost { get; set; }
    public decimal AnnualRevenue { get; set; }
    public decimal NetOperatingIncome { get; set; }
    public decimal? PaybackPeriod { get; set; }
    public decimal Npv { get; set; }
}

public class AssistantContextDto
{
    public SelectedLocationDto? SelectedLocation { get; set; }
    public CalculatorSnapshotDto? CalculatorSnapshot { get; set; }
}

public class AssistantRequestDto
{
    public string Question { get; set; } = string.Empty;
    public AssistantContextDto Context { get; set; } = new();
}

public class AssistantResponseDto
{
    public string Answer { get; set; } = string.Empty;
    public string? Error { get; set; }
}
