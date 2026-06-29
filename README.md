# Renewable Dashboard (.NET + Blazor + SQL)

A U.S. renewable energy investment analysis dashboard migrated from Next.js to **ASP.NET Core 8**, **Blazor Server**, and **SQL Server** (with SQLite for local development).

## Features

- **Home** — Live dashboard snapshot with market KPIs, selected location, and project NPV
- **Market** — EIA electricity price trends with interactive Chart.js line chart
- **Calculator** — Renewable project economics (NPV, NOI, payback, cash flow)
- **Map** — Leaflet map with SQL-backed state location data
- **AI Assistant** — OpenAI-powered investment analyst grounded in dashboard context

## Tech Stack

| Layer | Technology |
|-------|------------|
| Frontend | Blazor Server (interactive components) |
| Backend | ASP.NET Core 8 Web API |
| Database | SQL Server (production) / SQLite (development) |
| ORM | Entity Framework Core 8 |
| Charts | Chart.js |
| Map | Leaflet (JS interop) |
| AI | OpenAI GPT-4o-mini |

## Project Structure

```
src/
├── RenewableDashboard.Core/           # Models, DTOs, calculator logic
├── RenewableDashboard.Infrastructure/ # EF Core, EIA & OpenAI services
└── RenewableDashboard.Web/            # Blazor UI, API controllers, state services
database/
└── Schema.sql                         # SQL Server schema and seed data
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (production) — or use the built-in SQLite dev database
- API keys (optional):
  - `EIA_API_KEY` — [EIA Open Data API](https://www.eia.gov/opendata/)
  - `OPENAI_API_KEY` — [OpenAI API](https://platform.openai.com/)

## Getting Started

### 1. Configure API keys

Edit `src/RenewableDashboard.Web/appsettings.Development.json`:

```json
{
  "EIA_API_KEY": "your-eia-key",
  "OPENAI_API_KEY": "your-openai-key"
}
```

### 2. Database

**Development (default):** SQLite database is created automatically at `renewable.db`.

**Production (SQL Server):**

1. Run `database/Schema.sql` against your SQL Server instance
2. Update `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=RenewableDashboard;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Run the application

```bash
cd src/RenewableDashboard.Web
dotnet run
```

Open [http://localhost:5000](http://localhost:5000) (or the URL shown in the terminal).

## API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/market` | National electricity market data (EIA proxy) |
| POST | `/api/assistant` | AI assistant with dashboard context |

## Migration Notes

This application was converted from a Next.js/React dashboard. Key changes:

- React Context → Blazor scoped state services (`LocationStateService`, `CalculatorStateService`)
- Next.js API routes → ASP.NET Core controllers
- Static `locationData.ts` → SQL `Locations` table with EF Core seeding
- Recharts → Chart.js via JS interop
- react-leaflet → Leaflet via JS interop

## Build

```bash
dotnet build RenewableDashboard.sln
```
