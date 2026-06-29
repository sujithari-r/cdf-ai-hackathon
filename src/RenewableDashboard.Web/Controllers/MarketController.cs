using Microsoft.AspNetCore.Mvc;
using RenewableDashboard.Core.DTOs;
using RenewableDashboard.Core.Interfaces;

namespace RenewableDashboard.Web.Controllers;

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
    public async Task<ActionResult<MarketDataDto>> Get(CancellationToken cancellationToken)
    {
        var data = await _marketService.GetMarketDataAsync(cancellationToken);
        return Ok(data);
    }
}
