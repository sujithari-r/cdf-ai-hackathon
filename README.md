# Renewable Dashboard (.NET + SQL + Blazor)

This repository now contains a .NET 8 Blazor application with SQL persistence (SQLite via Entity Framework Core).

## Stack

- **Backend/Application**: ASP.NET Core / Blazor Server (.NET 8)
- **Database**: SQLite + Entity Framework Core
- **Frontend**: Blazor components/pages
- **External APIs**:
  - EIA Open Data API (market snapshots)
  - OpenAI Responses API (assistant)

## Project structure

- `RenewableDashboard.sln`
- `src/RenewableDashboard.Web/`
  - `Components/Pages/`
    - `Home.razor`
    - `Market.razor`
    - `Map.razor`
    - `Calculator.razor`
    - `Assistant.razor`
  - `Data/`
    - `AppDbContext.cs`
    - `DbInitializer.cs`
    - `Migrations/`
  - `Services/`
    - `MarketService.cs`
    - `CalculatorService.cs`
    - `AssistantService.cs`
    - `DashboardStateService.cs`

## Setup

1. Install .NET 8 SDK.
2. Configure settings in `src/RenewableDashboard.Web/appsettings.json` or environment variables:
   - `ConnectionStrings__DefaultConnection` (optional override)
   - `EIA_API_KEY` or `EiaApiKey`
   - `OPENAI_API_KEY` or `OpenAiApiKey`

## Run

```bash
dotnet restore RenewableDashboard.sln
dotnet run --project src/RenewableDashboard.Web/RenewableDashboard.Web.csproj
```

Open the app URL shown in the console.

## Database

- Migrations are included under `src/RenewableDashboard.Web/Data/Migrations`.
- On startup, migrations are applied automatically and location seed data is inserted.
