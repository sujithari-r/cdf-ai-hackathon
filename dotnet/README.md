# Renewable Dashboard — .NET / Blazor / SQL

This is a full conversion of the original Next.js **Renewable Energy Investment
Dashboard** into a **.NET 9 Blazor Web App** backed by a **SQL database** (via
Entity Framework Core).

It preserves the original feature set and visual design:

| Area | Original (Next.js) | This app (.NET / Blazor) |
| --- | --- | --- |
| Home dashboard | `app/page.tsx` | `Components/Pages/Home.razor` |
| Market trends + chart | `app/market/page.tsx` + `/api/market` | `Components/Pages/Market.razor` + `MarketService` |
| Project economics calculator | `app/calculator/page.tsx` | `Components/Pages/Calculator.razor` + `CalculatorService` |
| Opportunity map (Leaflet) | `app/map/*` | `Components/Pages/Map.razor` + `wwwroot/js/map.js` |
| AI assistant | `app/assistant/page.tsx` + `/api/assistant` | `Components/Pages/Assistant.razor` + `AssistantService` |
| Shared state (React Context) | `context/*` | `Services/DashboardState.cs` (scoped) |
| Location data | `lib/locationData.ts` | SQL `Locations` table (seeded) |

## Architecture

- **UI:** Blazor Web App with Interactive Server rendering. Styling uses the same
  Tailwind utility classes as the original (loaded via the Tailwind CDN in
  `Components/App.razor`).
- **Charts:** Chart.js via JS interop (`wwwroot/js/charts.js`) replaces Recharts.
- **Map:** Leaflet via JS interop (`wwwroot/js/map.js`) replaces react-leaflet.
- **Data:** Entity Framework Core. The default provider is **SQLite** so the app
  runs anywhere with zero setup. The schema is created/updated automatically on
  startup via `Database.MigrateAsync()`.
- **State:** `DashboardState` is a scoped service that holds the selected location
  and the calculator snapshot for a session (the equivalent of the React contexts).

### SQL tables

- `Locations` — tracked U.S. states (seeded from the original data).
- `MarketTrendPoints` / `MarketSummaries` — market trend + indicators (offline fallback).
- `CalculatorRuns` — saved calculator results (use the "Save run to database" button).
- `ChatMessages` — persisted assistant conversation history.

## Running

```bash
cd dotnet/RenewableDashboard
dotnet run
```

Then open the URL printed in the console (e.g. `http://localhost:5099`).

## Configuration

Set these via `appsettings.json`, environment variables, or user secrets:

- `ConnectionStrings:DefaultConnection` — EF Core connection string (defaults to
  `Data Source=renewable.db`).
- `EIA_API_KEY` — optional. When set, the Market page pulls live data from the
  EIA Open Data API; otherwise it uses the seeded trend from SQL.
- `OPENAI_API_KEY` — optional. When set, the assistant calls OpenAI
  (`gpt-4o-mini`); otherwise it returns a grounded, deterministic analysis built
  from the current dashboard context.

## Switching to SQL Server

The data layer is provider-agnostic. To target SQL Server instead of SQLite:

1. Add the package: `dotnet add package Microsoft.EntityFrameworkCore.SqlServer`
2. In `Program.cs`, replace `options.UseSqlite(...)` with `options.UseSqlServer(...)`.
3. In `Data/AppDbContextFactory.cs`, do the same for design-time migrations.
4. Set `ConnectionStrings:DefaultConnection` to your SQL Server connection string
   and regenerate migrations (`dotnet ef migrations add InitialCreate`).
