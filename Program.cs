using Microsoft.EntityFrameworkCore;
using RenewableDashboard.Components;
using RenewableDashboard.Data;
using RenewableDashboard.Models;
using RenewableDashboard.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDbContext<DashboardDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=renewable-dashboard.db"));
builder.Services.AddHttpClient<MarketService>();
builder.Services.AddHttpClient<AssistantService>();
builder.Services.AddScoped<LocationService>();
builder.Services.AddScoped<CalculatorService>();
builder.Services.AddScoped<DashboardState>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DashboardDbContext>();
    dbContext.Database.EnsureCreated();
}

app.MapGet("/api/market", async (MarketService marketService, CancellationToken cancellationToken) =>
{
    return Results.Ok(await marketService.GetMarketDataAsync(cancellationToken));
});

app.MapPost("/api/assistant", async (AssistantRequest request, AssistantService assistantService, CancellationToken cancellationToken) =>
{
    return Results.Ok(await assistantService.AskAsync(request, cancellationToken));
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
