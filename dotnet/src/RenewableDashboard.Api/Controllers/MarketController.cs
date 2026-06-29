using Microsoft.AspNetCore.Mvc;
using RenewableDashboard.Core.Interfaces;

namespace RenewableDashboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarketController : ControllerBase
{
    private readonly IMarketService _marketService;

    public MarketController(IMarketService marketService)
    {
        _marketService = marketService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMarketData()
    {
        var data = await _marketService.GetMarketDataAsync();
        return Ok(data);
    }
}
