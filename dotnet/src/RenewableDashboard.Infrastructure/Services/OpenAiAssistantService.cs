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

    public async Task<AssistantResponseDto> AskAsync(AssistantRequestDto request)
    {
        try
        {
            var apiKey = _configuration["OPENAI_API_KEY"] ?? string.Empty;
            var client = new OpenAIClient(apiKey);
            var chat = client.GetChatClient("gpt-4o-mini");

            var contextText = BuildContextText(request.Context);
            var systemPrompt = $"""
                You are a renewable energy investment analyst assistant. Use the dashboard data provided to give specific, actionable insights.
                
                {contextText}
                
                Structure your response as:
                **Key Insight:** [one sentence summary]
                **Financial Impact:** [specific numbers from context]
                **Recommendation:** [actionable next step]
                **Sources:** [data sources referenced]
                """;

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(request.Question)
            };

            var completion = await chat.CompleteChatAsync(messages);
            return new AssistantResponseDto { Answer = completion.Value.Content[0].Text };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to call OpenAI assistant");
            return new AssistantResponseDto { Answer = "I'm unable to process your question right now. Please check that a valid OpenAI API key is configured." };
        }
    }

    private static string BuildContextText(AssistantContextDto? context)
    {
        if (context == null) return string.Empty;

        var parts = new List<string>();

        if (context.SelectedLocation != null)
        {
            var loc = context.SelectedLocation;
            parts.Add($"Selected Location: {loc.Name} | Electricity Rate: ${loc.ElectricityRate:F4}/kWh | Solar Score: {loc.SolarScore}/100 | Note: {loc.Note}");
        }

        if (context.CalculatorSnapshot != null)
        {
            var snap = context.CalculatorSnapshot;
            parts.Add($"Calculator Results: Scenario={snap.Scenario} | Rate Mode={snap.RateMode} | Total Cost=${snap.TotalProjectCost:N0} | Annual Revenue=${snap.AnnualRevenue:N0} | NOI=${snap.NetOperatingIncome:N0} | Payback={snap.PaybackPeriod?.ToString("F1") ?? "N/A"} yrs | NPV=${snap.Npv:N0}");
        }

        return parts.Count > 0 ? "Current Dashboard Context:\n" + string.Join("\n", parts) : string.Empty;
    }
}
