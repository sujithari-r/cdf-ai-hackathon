using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;
using RenewableDashboard.Core.DTOs;
using RenewableDashboard.Core.Interfaces;

namespace RenewableDashboard.Infrastructure.Services;

public class OpenAiAssistantService : IAssistantService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<OpenAiAssistantService> _logger;

    public OpenAiAssistantService(IConfiguration configuration, ILogger<OpenAiAssistantService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AssistantResponseDto> GetAnswerAsync(
        AssistantRequestDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var apiKey = _configuration["OPENAI_API_KEY"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return new AssistantResponseDto
                {
                    Answer = "OpenAI API key is not configured. Add OPENAI_API_KEY to your configuration to enable the AI assistant.",
                    Error = "Missing API key"
                };
            }

            var contextSummary = BuildContextSummary(request.Context);
            var prompt = $"""
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
                {request.Question}
                """;

            var client = new OpenAIClient(apiKey);
            var chatClient = client.GetChatClient("gpt-4o-mini");
            var completion = await chatClient.CompleteChatAsync(
                [new UserChatMessage(prompt)],
                cancellationToken: cancellationToken);

            var answer = completion.Value.Content.FirstOrDefault()?.Text
                ?? "I could not generate a response at the moment.";

            return new AssistantResponseDto { Answer = answer };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process assistant request");

            return new AssistantResponseDto
            {
                Error = "Failed to process assistant request",
                Answer = "Something went wrong while generating the AI response. Please try again."
            };
        }
    }

    private static string BuildContextSummary(AssistantContextDto context)
    {
        var summary = string.Empty;
        var location = context.SelectedLocation;

        if (location is not null)
        {
            summary += $"""

                Selected Location:
                - Name: {location.Name}
                - Electricity Rate: ${location.ElectricityRate}/kWh
                - Solar Score: {location.SolarScore}/10
                - Note: {location.Note}
                """;
        }
        else
        {
            summary += """

                Selected Location:
                - None
                """;
        }

        var calculator = context.CalculatorSnapshot;
        if (calculator is not null)
        {
            var payback = calculator.PaybackPeriod.HasValue
                ? $"{calculator.PaybackPeriod.Value:F1} years"
                : "Not achievable";

            summary += $"""

                Calculator Snapshot:
                - Scenario: {calculator.Scenario}
                - Rate Mode: {calculator.RateMode}
                - Manual Electricity Rate: ${calculator.ManualElectricityRate}/kWh
                - Active Electricity Rate: ${calculator.ActiveElectricityRate}/kWh
                - Total Project Cost: ${calculator.TotalProjectCost}
                - Annual Revenue: ${calculator.AnnualRevenue}
                - Net Operating Income: ${calculator.NetOperatingIncome}
                - Payback Period: {payback}
                - NPV: ${calculator.Npv}
                """;
        }
        else
        {
            summary += """

                Calculator Snapshot:
                - None
                """;
        }

        return summary;
    }
}
