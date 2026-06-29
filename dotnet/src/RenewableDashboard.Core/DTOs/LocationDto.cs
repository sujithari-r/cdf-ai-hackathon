namespace RenewableDashboard.Core.DTOs;

public class LocationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double ElectricityRate { get; set; }
    public int SolarScore { get; set; }
    public string Note { get; set; } = string.Empty;
}
