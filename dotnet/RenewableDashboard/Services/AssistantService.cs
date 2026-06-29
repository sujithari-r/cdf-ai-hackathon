using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RenewableDashboard.Data;
using RenewableDashboard.Data.Entities;
using RenewableDashboard.Models;

namespace RenewableDashboard.Services;

/// <summary>
/// Renewable energy investment analyst assistant. Uses OpenAI when an API key is
/// configured and otherwise produces a grounded, deterministic answer from the
/// dashboard context. Ported from the original /api/assistant route.
/// </summary>
public class AssistantService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AssistantService> _logger;

    public AssistantService(
        IDbContextFactory<AppDbContext> dbFactory,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<AssistantService> logger)
    {
        _dbFactory = dbFactory;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> AskAsync(
        string question,
        SelectedLocation? location,
        CalculatorSnapshot? snapshot,
        string sessionId)
    {
        var contextSummary = BuildContextSummary(location, snapshot);
        var prompt = BuildPrompt(contextSummary, question);

        string answer;
        var apiKey = _configuration["OPENAI_API_KEY"]
            ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            answer = await TryOpenAiAsync(apiKey, prompt)
                ?? BuildHeuristicAnswer(question, location, snapshot);
        }
        else
        {
            answer = BuildHeuristicAnswer(question, location, snapshot);
        }

        await PersistAsync(sessionId, question, answer);
        return answer;
    }

    public async Task<List<ChatMessage>> GetHistoryAsync(string sessionId)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.ChatMessages
            .Where(m => m.SessionId == sessionId)
            .OrderBy(m => m.Id)
            .ToListAsync();
    }

    private async Task PersistAsync(string sessionId, string question, string answer)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            db.ChatMessages.Add(new ChatMessage
            {
                SessionId = sessionId,
                Role = "user",
                Content = question,
            });
            db.ChatMessages.Add(new ChatMessage
            {
                SessionId = sessionId,
                Role = "assistant",
                Content = answer,
            });
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to persist chat messages.");
        }
    }

    private async Task<string?> TryOpenAiAsync(string apiKey, string prompt)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(30);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            request.Headers.Add("Authorization", $"Bearer {apiKey}");

            var payload = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "user", content = prompt },
                },
            };

            request.Content = new StringContent(
                JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            var content = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return string.IsNullOrWhiteSpace(content) ? null : content;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "OpenAI request failed; using heuristic answer.");
            return null;
        }
    }

    private static string BuildContextSummary(SelectedLocation? location, CalculatorSnapshot? snapshot)
    {
        var sb = new StringBuilder();

        if (location is not null)
        {
            sb.AppendLine("Selected Location:");
            sb.AppendLine($"- Name: {location.Name}");
            sb.AppendLine($"- Electricity Rate: ${location.ElectricityRate}/kWh");
            sb.AppendLine($"- Solar Score: {location.SolarScore}/10");
            sb.AppendLine($"- Note: {location.Note}");
        }
        else
        {
            sb.AppendLine("Selected Location:");
            sb.AppendLine("- None");
        }

        if (snapshot is not null)
        {
            sb.AppendLine("Calculator Snapshot:");
            sb.AppendLine($"- Scenario: {snapshot.Scenario}");
            sb.AppendLine($"- Rate Mode: {snapshot.RateMode}");
            sb.AppendLine($"- Manual Electricity Rate: ${snapshot.ManualElectricityRate}/kWh");
            sb.AppendLine($"- Active Electricity Rate: ${snapshot.ActiveElectricityRate}/kWh");
            sb.AppendLine($"- Total Project Cost: ${snapshot.TotalProjectCost:N0}");
            sb.AppendLine($"- Annual Revenue: ${snapshot.AnnualRevenue:N0}");
            sb.AppendLine($"- Net Operating Income: ${snapshot.NetOperatingIncome:N0}");
            sb.AppendLine(snapshot.PaybackPeriod.HasValue
                ? $"- Payback Period: {snapshot.PaybackPeriod.Value:F1} years"
                : "- Payback Period: Not achievable");
            sb.AppendLine($"- NPV: ${snapshot.Npv:N0}");
        }
        else
        {
            sb.AppendLine("Calculator Snapshot:");
            sb.AppendLine("- None");
        }

        return sb.ToString();
    }

    private static string BuildPrompt(string contextSummary, string question) =>
$@"You are a renewable energy investment analyst AI embedded inside a dashboard.

STRICT RULES:
- You MUST prioritize the provided dashboard context over general knowledge.
- If location or calculator data exists, you MUST use it in your answer.
- DO NOT give generic answers when context is available.
- If no context is available, then use general knowledge.
- Always reference numbers from the dashboard when present.

Response format:
1. Key Insight
2. Financial Impact
3. Recommendation
4. Sources used

Dashboard Context:
{contextSummary}

User Question:
{question}";

    /// <summary>
    /// Deterministic, context-grounded answer used when no OpenAI key is configured.
    /// </summary>
    private static string BuildHeuristicAnswer(
        string question,
        SelectedLocation? location,
        CalculatorSnapshot? snapshot)
    {
        var sb = new StringBuilder();

        sb.AppendLine("1. Key Insight");
        if (snapshot is not null)
        {
            var viable = snapshot.Npv > 0;
            sb.AppendLine(
                $"Based on the current {snapshot.Scenario} scenario, this project's NPV is " +
                $"${snapshot.Npv:N0}, which suggests the project is currently " +
                $"{(viable ? "value-accretive" : "not yet financially viable")} at an active rate of " +
                $"${snapshot.ActiveElectricityRate:F3}/kWh.");
        }
        else if (location is not null)
        {
            sb.AppendLine(
                $"{location.Name} has an electricity rate of ${location.ElectricityRate}/kWh and a solar " +
                $"score of {location.SolarScore}/10, which frames its renewable opportunity.");
        }
        else
        {
            sb.AppendLine(
                "No dashboard context is selected yet. Open the Calculator or pick a state on the Map to " +
                "ground this analysis in real project numbers.");
        }

        sb.AppendLine();
        sb.AppendLine("2. Financial Impact");
        if (snapshot is not null)
        {
            sb.AppendLine(
                $"Annual revenue is ${snapshot.AnnualRevenue:N0} and net operating income is " +
                $"${snapshot.NetOperatingIncome:N0}. " +
                (snapshot.PaybackPeriod.HasValue
                    ? $"Simple payback lands around {snapshot.PaybackPeriod.Value:F1} years."
                    : "Payback is not achievable under the current assumptions."));
        }
        else
        {
            sb.AppendLine(
                "Run the Calculator to produce revenue, NOI, payback, and NPV figures for a concrete read.");
        }

        sb.AppendLine();
        sb.AppendLine("3. Recommendation");
        if (location is not null && snapshot is not null)
        {
            sb.AppendLine(
                $"Consider stress-testing {location.Name}'s ${location.ElectricityRate}/kWh rate against your " +
                "manual assumption using Compare mode, and review the optimistic scenario for upside.");
        }
        else
        {
            sb.AppendLine(
                "Start with the Market tab for pricing direction, choose a state on the Map, then model returns " +
                "in the Calculator before forming a view.");
        }

        sb.AppendLine();
        sb.AppendLine("4. Sources used");
        sb.AppendLine(
            "Dashboard context (selected location and calculator snapshot) plus seeded U.S. market indicators.");

        return sb.ToString().TrimEnd();
    }
}
