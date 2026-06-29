using Microsoft.AspNetCore.Mvc;
using RenewableDashboard.Core.DTOs;
using RenewableDashboard.Core.Entities;
using RenewableDashboard.Core.Interfaces;

namespace RenewableDashboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly ILocationRepository _repository;

    public LocationsController(ILocationRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var locations = await _repository.GetAllAsync();
        var dtos = locations.Select(l => new LocationDto
        {
            Id = l.Id,
            Name = l.Name,
            Latitude = l.Latitude,
            Longitude = l.Longitude,
            ElectricityRate = l.ElectricityRate,
            SolarScore = l.SolarScore,
            Note = l.Note
        });
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var location = await _repository.GetByIdAsync(id);
        if (location == null) return NotFound();

        return Ok(new LocationDto
        {
            Id = location.Id,
            Name = location.Name,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            ElectricityRate = location.ElectricityRate,
            SolarScore = location.SolarScore,
            Note = location.Note
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LocationDto dto)
    {
        var location = new Location
        {
            Name = dto.Name,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            ElectricityRate = dto.ElectricityRate,
            SolarScore = dto.SolarScore,
            Note = dto.Note
        };

        var created = await _repository.AddAsync(location);
        dto.Id = created.Id;
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, dto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
