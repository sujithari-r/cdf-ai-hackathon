namespace RenewableDashboard.Models;

/// <summary>The currently selected map location shared across the dashboard.</summary>
public class SelectedLocation
{
    public string Name { get; set; } = string.Empty;
    public double ElectricityRate { get; set; }
    public double SolarScore { get; set; }
    public string Note { get; set; } = string.Empty;
}
