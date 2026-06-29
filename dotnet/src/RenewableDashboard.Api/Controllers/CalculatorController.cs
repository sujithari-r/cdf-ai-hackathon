using Microsoft.AspNetCore.Mvc;
using RenewableDashboard.Core.DTOs;
using RenewableDashboard.Core.Services;

namespace RenewableDashboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalculatorController : ControllerBase
{
    private readonly CalculatorService _calculatorService;

    public CalculatorController(CalculatorService calculatorService)
    {
        _calculatorService = calculatorService;
    }

    [HttpPost("calculate")]
    public IActionResult Calculate([FromBody] CalculatorInputDto input)
    {
        var result = _calculatorService.Calculate(input);
        return Ok(result);
    }
}
