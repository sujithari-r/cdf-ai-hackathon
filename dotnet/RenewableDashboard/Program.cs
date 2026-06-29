using Microsoft.EntityFrameworkCore;
using RenewableDashboard.Components;
using RenewableDashboard.Data;
using RenewableDashboard.Services;

var builder = WebApplication.CreateBuilder(args);

// Razor components with interactive server rendering.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// SQL data access via EF Core (SQLite by default; swap the provider below for
// SQL Server by using UseSqlServer with the "SqlServer" connection string).
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=renewable.db";

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddHttpClient();

// Application services.
builder.Services.AddScoped<DashboardState>();
builder.Services.AddScoped<CalculatorService>();
builder.Services.AddScoped<LocationService>();
builder.Services.AddScoped<MarketService>();
builder.Services.AddScoped<AssistantService>();

var app = builder.Build();

// Ensure the database exists and is up to date.
using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    await using var db = await factory.CreateDbContextAsync();
    await db.Database.MigrateAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
