# Renewable Dashboard — .NET + SQL Server + Blazor

This is the .NET 8 conversion of the Renewable Energy Investment Dashboard, previously built with Next.js/React. It uses:

- **ASP.NET Core 8** Web API backend
- **Blazor Server** with **MudBlazor** for the frontend
- **Entity Framework Core 8** with **SQL Server**
- **OpenAI SDK** for the AI assistant
- **EIA Open Data API** for live electricity price data

## Solution Structure

```
RenewableDashboard.sln
└── src/
    ├── RenewableDashboard.Core/            # Domain models, DTOs, interfaces, calculator service
    ├── RenewableDashboard.Infrastructure/  # EF Core DbContext, repositories, EIA & OpenAI services
    ├── RenewableDashboard.Api/             # ASP.NET Core Web API (market, assistant, locations, calculator)
    └── RenewableDashboard.Blazor/          # Blazor Server frontend (MudBlazor UI)
```

## Pages

| Page | Route | Description |
|------|-------|-------------|
| Dashboard | `/` | KPI cards: electricity price, capacity growth, renewable share, NPV |
| Market Analysis | `/market` | EIA price trend table with 6M/ALL filter |
| Calculator | `/calculator` | 20-year NPV/payback financial model with save-to-DB |
| Location Map | `/map` | Select U.S. state; rates flow into calculator |
| AI Assistant | `/assistant` | OpenAI-powered chat with dashboard context |
| Saved Scenarios | `/scenarios` | Review and delete saved calculator runs from SQL |

## API Endpoints

| Method | Path | Description |
|--------|------|-------------|
| `GET` | `/api/market` | Fetch EIA electricity price data (cached in SQL) |
| `POST` | `/api/assistant` | Ask OpenAI assistant with dashboard context |
| `GET` | `/api/locations` | List all locations from SQL |
| `POST` | `/api/locations` | Add a location |
| `DELETE` | `/api/locations/{id}` | Delete a location |
| `POST` | `/api/calculator/calculate` | Run financial model |

## SQL Server Database Schema

| Table | Purpose |
|-------|---------|
| `Locations` | U.S. states with electricity rate + solar score (seeded with TX, CA, AZ) |
| `MarketSnapshots` | Cached EIA API responses (refreshed hourly) |
| `MarketTrends` | Monthly price history linked to snapshots |
| `ProjectScenarios` | Saved calculator results (NPV, payback, NOI, inputs) |

## Prerequisites

- [.NET 8 SDK](https://dot.net)
- SQL Server (local or Docker)
- EIA API key — [free at eia.gov](https://www.eia.gov/opendata/)
- OpenAI API key — [platform.openai.com](https://platform.openai.com)

## Quick Start (Local)

### 1. Configure secrets

Edit `appsettings.json` in both `RenewableDashboard.Api` and `RenewableDashboard.Blazor`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=RenewableDashboard;Trusted_Connection=True;"
  },
  "EIA_API_KEY": "your-eia-key",
  "OPENAI_API_KEY": "your-openai-key"
}
```

Or use environment variables / `dotnet user-secrets`.

### 2. Run database migrations

Migrations are applied automatically on startup. To apply manually:

```bash
dotnet ef database update \
  --project src/RenewableDashboard.Infrastructure \
  --startup-project src/RenewableDashboard.Api
```

### 3. Run the applications

**API (optional — Blazor has its own service registration):**
```bash
dotnet run --project src/RenewableDashboard.Api
# → http://localhost:5000  (Swagger UI at /swagger)
```

**Blazor frontend:**
```bash
dotnet run --project src/RenewableDashboard.Blazor
# → http://localhost:5001
```

## Quick Start (Docker)

```bash
# Set your API keys
export EIA_API_KEY=your-eia-key
export OPENAI_API_KEY=your-openai-key

# Start everything (SQL Server + API + Blazor)
docker-compose up --build

# Blazor UI: http://localhost:5001
# API + Swagger: http://localhost:5000/swagger
```

## Financial Model

The calculator implements a 20-year discounted cash flow model:

```
Total Project Cost  = system_kW × 1000 × install_cost_$/W
Annual Production   = system_kW × 8760 × capacity_factor
Annual Revenue      = production × electricity_rate
Net Operating Income = revenue − O&M cost
Payback Period      = cost / NOI
NPV (8%, 20yr)      = −cost + Σ(NOI / (1.08)^year)
```

**Optimistic scenario** adjustments: +3% capacity factor, −5% O&M, +$0.02/kWh rate.

## Key Technologies

| Package | Version | Role |
|---------|---------|------|
| Microsoft.EntityFrameworkCore.SqlServer | 8.x | ORM + SQL Server driver |
| MudBlazor | 7.x | Blazor UI component library |
| OpenAI | 2.x | OpenAI .NET SDK |
| Microsoft.AspNetCore (built-in) | 8.0 | Web API + Blazor Server |
