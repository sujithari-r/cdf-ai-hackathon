using Microsoft.AspNetCore.Mvc;
using RenewableDashboard.Core.DTOs;
using RenewableDashboard.Core.Interfaces;

namespace RenewableDashboard.Api.Controllers;

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
    public async Task<IActionResult> Ask([FromBody] AssistantRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
            return BadRequest(new { error = "Question is required" });

        var response = await _assistantService.AskAsync(request);
        return Ok(response);
    }
}
