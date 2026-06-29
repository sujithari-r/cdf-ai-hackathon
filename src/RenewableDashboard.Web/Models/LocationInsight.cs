namespace RenewableDashboard.Web.Models;

public class LocationInsight
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal ElectricityRate { get; set; }
    public decimal SolarScore { get; set; }
    public string Note { get; set; } = string.Empty;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}
