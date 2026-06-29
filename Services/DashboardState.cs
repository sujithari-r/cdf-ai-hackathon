using RenewableDashboard.Models;

namespace RenewableDashboard.Services;

public sealed class DashboardState
{
    public Location? SelectedLocation { get; private set; }
    public CalculatorSnapshot? CalculatorSnapshot { get; private set; }

    public event Action? OnChange;

    public void SetSelectedLocation(Location location)
    {
        SelectedLocation = location;
        NotifyStateChanged();
    }

    public void SetCalculatorSnapshot(CalculatorSnapshot snapshot)
    {
        CalculatorSnapshot = snapshot;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
