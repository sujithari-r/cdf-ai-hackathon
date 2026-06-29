namespace RenewableDashboard.Data.Entities;

/// <summary>
/// A persisted message from an AI assistant conversation.
/// </summary>
public class ChatMessage
{
    public int Id { get; set; }

    public string SessionId { get; set; } = string.Empty;

    /// <summary>"user" or "assistant".</summary>
    public string Role { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
