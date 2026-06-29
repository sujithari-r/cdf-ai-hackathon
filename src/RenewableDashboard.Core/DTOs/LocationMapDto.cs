namespace RenewableDashboard.Core.DTOs;

public class LocationMapDto
{
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public decimal ElectricityRate { get; set; }
    public decimal SolarScore { get; set; }
    public string Note { get; set; } = string.Empty;
}
