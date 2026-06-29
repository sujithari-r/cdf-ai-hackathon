namespace RenewableDashboard.Models;

public sealed record AssistantRequest(string Question, AssistantContext? Context);

public sealed record AssistantContext(Location? SelectedLocation, CalculatorSnapshot? CalculatorSnapshot);

public sealed record AssistantResponse(string Answer, string? Error = null);

public sealed record ChatMessage(string Role, string Content, IReadOnlyList<string>? Suggestions = null);
