using MudBlazor.Services;
using RenewableDashboard.Blazor.Components;
using RenewableDashboard.Blazor.Services;
using RenewableDashboard.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<DashboardStateService>();

var app = builder.Build();

await DependencyInjection.InitializeDatabaseAsync(app.Services);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
