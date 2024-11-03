using Microsoft.AspNetCore.Mvc;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationsController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationsController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet]
    [Route("/NextLocation/{orderId}")]
    public async Task<ActionResult<Location>> GetNextLocation(int orderId) //TODO order comes from loggedIn User.CurrentOrder
        => await _locationService.QueryNextLocation(orderId);
}