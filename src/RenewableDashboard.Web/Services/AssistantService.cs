using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using RenewableDashboard.Web.Models;

namespace RenewableDashboard.Web.Services;

public class AssistantService(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration)
{
    public async Task<string> AskAsync(
        string question,
        LocationInsight? selectedLocation,
        CalculationSnapshot? snapshot,
        CancellationToken cancellationToken = default)
    {
        var apiKey = configuration["OpenAiApiKey"] ?? configuration["OPENAI_API_KEY"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return "OpenAI API key is not configured. Add OPENAI_API_KEY to enable live assistant responses.";
        }

        var prompt = BuildPrompt(question, selectedLocation, snapshot);
        var client = httpClientFactory.CreateClient(nameof(AssistantService));
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/responses");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Content = new StringContent(
            JsonSerializer.Serialize(new
            {
                model = "gpt-4o-mini",
                input = prompt
            }),
            Encoding.UTF8,
            "application/json");

        try
        {
            using var response = await client.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

            if (document.RootElement.TryGetProperty("output_text", out var outputText))
            {
                var content = outputText.GetString();
                if (!string.IsNullOrWhiteSpace(content))
                {
                    return content;
                }
            }

            return "The assistant did not return content for this request.";
        }
        catch
        {
            return "Something went wrong while contacting the AI assistant.";
        }
    }

    private static string BuildPrompt(
        string question,
        LocationInsight? selectedLocation,
        CalculationSnapshot? snapshot)
    {
        var locationSection = selectedLocation is null
            ? "Selected Location: None"
            : $"""
               Selected Location:
               - Name: {selectedLocation.Name}
               - Electricity Rate: ${selectedLocation.ElectricityRate}/kWh
               - Solar Score: {selectedLocation.SolarScore}/10
               - Note: {selectedLocation.Note}
               """;

        var snapshotSection = snapshot is null
            ? "Calculator Snapshot: None"
            : $"""
               Calculator Snapshot:
               - Scenario: {snapshot.Scenario}
               - Rate Mode: {snapshot.RateMode}
               - Manual Electricity Rate: ${snapshot.ManualElectricityRate}/kWh
               - Active Electricity Rate: ${snapshot.ActiveElectricityRate}/kWh
               - Total Project Cost: ${snapshot.TotalProjectCost}
               - Annual Revenue: ${snapshot.AnnualRevenue}
               - Net Operating Income: ${snapshot.NetOperatingIncome}
               - Payback Period: {(snapshot.PaybackPeriodYears.HasValue ? $"{snapshot.PaybackPeriodYears:F1} years" : "Not achievable")}
               - NPV: ${snapshot.Npv}
               """;

        return $"""
                You are a renewable energy investment analyst AI embedded inside a dashboard.

                STRICT RULES:
                - Prioritize provided dashboard context over general knowledge.
                - If location or calculator data exists, use it explicitly in your answer.
                - Avoid generic responses when contextual numbers are available.
                - If no context exists, provide cautious general guidance.

                Response format:
                1. Key Insight
                2. Financial Impact
                3. Recommendation
                4. Sources used

                Dashboard Context:
                {locationSection}
                {snapshotSection}

                User Question:
                {question}
                """;
    }
}
