using Microsoft.AspNetCore.Mvc;
using RenewableDashboard.Core.DTOs;
using RenewableDashboard.Core.Interfaces;

namespace RenewableDashboard.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssistantController : ControllerBase
{
    private readonly IAssistantService _assistantService;

    public AssistantController(IAssistantService assistantService)
    {
        _assistantService = assistantService;
    }

    [HttpPost]
    public async Task<ActionResult<AssistantResponseDto>> Post(
        [FromBody] AssistantRequestDto request,
        CancellationToken cancellationToken)
    {
        var response = await _assistantService.GetAnswerAsync(request, cancellationToken);
        if (!string.IsNullOrEmpty(response.Error) && response.Answer.Contains("went wrong"))
        {
            return StatusCode(500, response);
        }

        return Ok(response);
    }
}
