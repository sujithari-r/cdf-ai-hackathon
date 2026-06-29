namespace RenewableDashboard.Models;

public sealed class Location
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public decimal ElectricityRate { get; set; }
    public double SolarScore { get; set; }
    public string Note { get; set; } = string.Empty;
}
