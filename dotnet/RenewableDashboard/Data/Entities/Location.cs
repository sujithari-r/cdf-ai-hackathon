namespace RenewableDashboard.Data.Entities;

/// <summary>
/// A tracked U.S. location used for map-based electricity pricing and solar scoring.
/// </summary>
public class Location
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public double ElectricityRate { get; set; }

    public double SolarScore { get; set; }

    public string Note { get; set; } = string.Empty;
}
