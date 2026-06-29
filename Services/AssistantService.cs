using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using RenewableDashboard.Models;

namespace RenewableDashboard.Services;

public sealed class AssistantService(HttpClient httpClient, IConfiguration configuration, ILogger<AssistantService> logger)
{
    public async Task<AssistantResponse> AskAsync(AssistantRequest request, CancellationToken cancellationToken = default)
    {
        var apiKey = configuration["OPENAI_API_KEY"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return new AssistantResponse(
                "OPENAI_API_KEY is not configured. Add it to environment variables or user secrets to enable AI responses.",
                "Missing API key");
        }

        try
        {
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/responses");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            httpRequest.Content = new StringContent(
                JsonSerializer.Serialize(new
                {
                    model = "gpt-4o-mini",
                    input = BuildPrompt(request.Question, request.Context)
                }),
                Encoding.UTF8,
                "application/json");

            using var response = await httpClient.SendAsync(httpRequest, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

            var answer = document.RootElement.TryGetProperty("output_text", out var outputText)
                ? outputText.GetString()
                : null;

            return new AssistantResponse(answer ?? "I could not generate a response at the moment.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to process assistant request.");

            return new AssistantResponse(
                "Something went wrong while generating the AI response. Please try again.",
                "Failed to process assistant request");
        }
    }

    private static string BuildPrompt(string question, AssistantContext? context)
    {
        var location = context?.SelectedLocation;
        var calculator = context?.CalculatorSnapshot;
        var contextSummary = new StringBuilder();

        if (location is not null)
        {
            contextSummary.AppendLine("Selected Location:");
            contextSummary.AppendLine($"- Name: {location.Name}");
            contextSummary.AppendLine($"- Electricity Rate: ${location.ElectricityRate}/kWh");
            contextSummary.AppendLine($"- Solar Score: {location.SolarScore}/10");
            contextSummary.AppendLine($"- Note: {location.Note}");
        }
        else
        {
            contextSummary.AppendLine("Selected Location:");
            contextSummary.AppendLine("- None");
        }

        if (calculator is not null)
        {
            contextSummary.AppendLine();
            contextSummary.AppendLine("Calculator Snapshot:");
            contextSummary.AppendLine($"- Scenario: {calculator.Scenario}");
            contextSummary.AppendLine($"- Rate Mode: {calculator.RateMode}");
            contextSummary.AppendLine($"- Manual Electricity Rate: ${calculator.ManualElectricityRate}/kWh");
            contextSummary.AppendLine($"- Active Electricity Rate: ${calculator.ActiveElectricityRate}/kWh");
            contextSummary.AppendLine($"- Total Project Cost: ${calculator.TotalProjectCost}");
            contextSummary.AppendLine($"- Annual Revenue: ${calculator.AnnualRevenue}");
            contextSummary.AppendLine($"- Net Operating Income: ${calculator.NetOperatingIncome}");
            contextSummary.AppendLine($"- Payback Period: {(calculator.PaybackPeriod is double payback ? $"{payback:F1} years" : "Not achievable")}");
            contextSummary.AppendLine($"- NPV: ${calculator.NetPresentValue}");
        }
        else
        {
            contextSummary.AppendLine();
            contextSummary.AppendLine("Calculator Snapshot:");
            contextSummary.AppendLine("- None");
        }

        return $"""
            You are a renewable energy investment analyst AI embedded inside a dashboard.

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
            {question}
            """;
    }
}
