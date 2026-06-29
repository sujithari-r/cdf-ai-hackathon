# Renewable Dashboard

Renewable Dashboard is an ASP.NET Core Blazor Web App with SQL-backed data access. It ports the original renewable investment dashboard from Next.js/React into a .NET application with:

- Blazor pages for Home, Market, Map, Calculator, and AI Assistant
- ASP.NET Core services for EIA market data, OpenAI assistant responses, and calculator metrics
- SQLite persistence via Entity Framework Core for tracked renewable locations and cached market snapshots
- Minimal API endpoints compatible with the previous `/api/market` and `/api/assistant` backend surface

## Requirements

- .NET SDK 8.0+
- Optional: `EIA_API_KEY` for live EIA market data
- Optional: `OPENAI_API_KEY` for assistant responses

## Getting Started

Restore and run the Blazor application:

```bash
dotnet restore
dotnet run
```

Open the URL printed by `dotnet run` in your browser.

The app creates `renewable-dashboard.db` automatically on startup and seeds the tracked locations:

- Texas
- California
- Arizona

## Configuration

Configuration can come from `appsettings.json`, environment variables, user secrets, or your hosting platform:

```bash
export ConnectionStrings__DefaultConnection="Data Source=renewable-dashboard.db"
export EIA_API_KEY="your-eia-key"
export OPENAI_API_KEY="your-openai-key"
```

## Project Structure

- `Components/Pages/` - Blazor route components
- `Components/Layout/` - application shell and navigation
- `Data/DashboardDbContext.cs` - EF Core SQL context and seed data
- `Models/` - location, market, calculator, and assistant models
- `Services/` - app state, calculator logic, EIA market integration, OpenAI integration, and SQL location lookup

## API Endpoints

- `GET /api/market` returns current price, growth, renewable share, and trend data
- `POST /api/assistant` accepts a dashboard-grounded question and returns an assistant answer
