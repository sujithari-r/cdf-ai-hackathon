using RenewableDashboard.Core.DTOs;

namespace RenewableDashboard.Blazor.Services;

public class DashboardStateService
{
    public LocationDto? SelectedLocation { get; private set; }
    public CalculatorResultDto? CalculatorSnapshot { get; private set; }
    public CalculatorInputDto? LastCalculatorInput { get; private set; }

    public event Action? OnChange;

    public void SetLocation(LocationDto? location)
    {
        SelectedLocation = location;
        NotifyStateChanged();
    }

    public void SetCalculatorResult(CalculatorResultDto result, CalculatorInputDto input)
    {
        CalculatorSnapshot = result;
        LastCalculatorInput = input;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
