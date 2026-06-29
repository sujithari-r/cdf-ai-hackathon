using RenewableDashboard.Web.Models;

namespace RenewableDashboard.Web.Services;

public class DashboardStateService
{
    public LocationInsight? SelectedLocation { get; private set; }
    public CalculationSnapshot? CalculatorSnapshot { get; private set; }

    public event Action? StateChanged;

    public void SetSelectedLocation(LocationInsight? location)
    {
        SelectedLocation = location;
        StateChanged?.Invoke();
    }

    public void SetCalculatorSnapshot(CalculationSnapshot? snapshot)
    {
        CalculatorSnapshot = snapshot;
        StateChanged?.Invoke();
    }
}
