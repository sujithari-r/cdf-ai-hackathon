namespace RenewableDashboard.Core.DTOs;

public class AssistantRequestDto
{
    public string Question { get; set; } = string.Empty;
    public AssistantContextDto? Context { get; set; }
}

public class AssistantContextDto
{
    public SelectedLocationDto? SelectedLocation { get; set; }
    public CalculatorSnapshotDto? CalculatorSnapshot { get; set; }
}

public class SelectedLocationDto
{
    public string Name { get; set; } = string.Empty;
    public double ElectricityRate { get; set; }
    public int SolarScore { get; set; }
    public string Note { get; set; } = string.Empty;
}

public class CalculatorSnapshotDto
{
    public string Scenario { get; set; } = "base";
    public string RateMode { get; set; } = "manual";
    public double ManualElectricityRate { get; set; }
    public double ActiveElectricityRate { get; set; }
    public double TotalProjectCost { get; set; }
    public double AnnualRevenue { get; set; }
    public double NetOperatingIncome { get; set; }
    public double? PaybackPeriod { get; set; }
    public double Npv { get; set; }
}

public class AssistantResponseDto
{
    public string Answer { get; set; } = string.Empty;
}
