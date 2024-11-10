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
    [Route("/NextLocation")]
    public async Task<ActionResult<Location>> GetNextLocation() // order comes from loggedIn User.CurrentOrder
    {
        var location = await _locationService.QueryNextLocation();
        HttpContext.Session.SetInt32("ExpectedLocationId", location.Id);
        return location;
    }

    [HttpPost]
    [Route("/VerifyLocation")]
    public void VerifyLocation(int locationId)
    {
        var expectedLocationId = HttpContext.Session.GetInt32("ExpectedLocationId");

        if (expectedLocationId == null || expectedLocationId != locationId)
            throw new ArgumentException("Incorrect location scanned. Please verify and try again.");
    }

    [HttpGet]
    [Route("/{locationId}")]
    public async Task<Location> GetLocationById(int locationId)
        => await _locationService.QueryLocationById(locationId);
}