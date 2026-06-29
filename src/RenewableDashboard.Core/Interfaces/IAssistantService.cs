using RenewableDashboard.Core.DTOs;

namespace RenewableDashboard.Core.Interfaces;

public interface IAssistantService
{
    Task<AssistantResponseDto> GetAnswerAsync(AssistantRequestDto request, CancellationToken cancellationToken = default);
}
