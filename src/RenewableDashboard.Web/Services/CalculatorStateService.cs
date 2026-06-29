using RenewableDashboard.Core.DTOs;

namespace RenewableDashboard.Web.Services;

public class CalculatorStateService
{
    public CalculatorSnapshotDto? Snapshot { get; private set; }

    public event Action? OnChange;

    public void SetSnapshot(CalculatorSnapshotDto snapshot)
    {
        Snapshot = snapshot;
        OnChange?.Invoke();
    }
}
