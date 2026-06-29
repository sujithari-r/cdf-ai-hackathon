using RenewableDashboard.Models;

namespace RenewableDashboard.Services;

/// <summary>
/// Per-circuit shared state replacing the original React LocationContext and
/// CalculatorContext. Registered as scoped so all pages in a Blazor Server
/// session observe the same selection and calculator snapshot.
/// </summary>
public class DashboardState
{
    public SelectedLocation? SelectedLocation { get; private set; }

    public CalculatorSnapshot? CalculatorSnapshot { get; private set; }

    /// <summary>Persistent inputs so calculator settings survive navigation.</summary>
    public CalculatorInputs CalculatorInputs { get; } = new();

    /// <summary>Stable id used to group persisted assistant messages.</summary>
    public string SessionId { get; } = Guid.NewGuid().ToString("N");

    public event Action? OnChange;

    public void SetSelectedLocation(SelectedLocation? location)
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
