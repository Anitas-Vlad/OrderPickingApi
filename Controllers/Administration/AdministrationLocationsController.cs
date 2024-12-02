using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers.Administration;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("[controller]")]
public class AdministrationLocationsController
{
    private readonly ILocationService _locationService;

    public AdministrationLocationsController(ILocationService locationService)
    {
        _locationService = locationService;
    }
    
    [HttpGet]
    [Route("/{locationId}")]
    public async Task<Location> GetLocationById(int locationId)
        => await _locationService.QueryLocationById(locationId);
}