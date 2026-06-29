using Microsoft.JSInterop;
using RenewableDashboard.Core.DTOs;

namespace RenewableDashboard.Web.Services;

public class MapJsInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public MapJsInterop(IJSRuntime jsRuntime)
    {
        _moduleTask = new Lazy<Task<IJSObjectReference>>(() =>
            jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/map.js").AsTask());
    }

    public async ValueTask InitializeMapAsync(
        string elementId,
        LocationMapDto[] locations,
        DotNetObjectReference<MapJsInterop> dotNetRef)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("initializeMap", elementId, locations, dotNetRef);
    }

    [JSInvokable]
    public void OnLocationSelected(SelectedLocationDto location)
    {
        LocationSelected?.Invoke(location);
    }

    public event Action<SelectedLocationDto>? LocationSelected;

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.InvokeVoidAsync("destroyMap");
            await module.DisposeAsync();
        }
    }
}
