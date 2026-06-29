using RenewableDashboard.Core.DTOs;

namespace RenewableDashboard.Web.Services;

public class LocationStateService
{
    public SelectedLocationDto? SelectedLocation { get; private set; }

    public event Action? OnChange;

    public void SetSelectedLocation(SelectedLocationDto location)
    {
        SelectedLocation = location;
        OnChange?.Invoke();
    }

    public void Clear()
    {
        SelectedLocation = null;
        OnChange?.Invoke();
    }
}
