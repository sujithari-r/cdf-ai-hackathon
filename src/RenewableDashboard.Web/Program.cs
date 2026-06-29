using RenewableDashboard.Web.Components;
using RenewableDashboard.Web.Services;
using RenewableDashboard.Core.Services;
using RenewableDashboard.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();
builder.Services.AddHttpClient();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=renewable.db";
var useSqlite = connectionString.Contains("Data Source=", StringComparison.OrdinalIgnoreCase)
    && !connectionString.Contains("Server=", StringComparison.OrdinalIgnoreCase);

builder.Services.AddInfrastructure(connectionString, useSqlite);
builder.Services.AddHttpClient<RenewableDashboard.Infrastructure.Services.EiaMarketService>();
builder.Services.AddSingleton<CalculatorService>();
builder.Services.AddScoped<LocationStateService>();
builder.Services.AddScoped<CalculatorStateService>();
builder.Services.AddScoped<ChartJsInterop>();
builder.Services.AddScoped<MapJsInterop>();

var app = builder.Build();

await app.Services.InitializeDatabaseAsync();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
