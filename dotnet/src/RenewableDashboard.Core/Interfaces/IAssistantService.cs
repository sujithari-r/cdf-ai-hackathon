using RenewableDashboard.Core.DTOs;

namespace RenewableDashboard.Core.Interfaces;

public interface IAssistantService
{
    Task<AssistantResponseDto> AskAsync(AssistantRequestDto request);
}
