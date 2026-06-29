using Microsoft.JSInterop;

namespace RenewableDashboard.Web.Services;

public class ChartJsInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public ChartJsInterop(IJSRuntime jsRuntime)
    {
        _moduleTask = new Lazy<Task<IJSObjectReference>>(() =>
            jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/charts.js").AsTask());
    }

    public async ValueTask CreateLineChartAsync(
        string canvasId,
        string[] labels,
        decimal[] values,
        string label)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("createLineChart", canvasId, labels, values, label);
    }

    public async ValueTask CreateBarChartAsync(
        string canvasId,
        string[] labels,
        decimal[] values)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("createBarChart", canvasId, labels, values);
    }

    public async ValueTask DestroyChartAsync(string canvasId)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("destroyChart", canvasId);
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
