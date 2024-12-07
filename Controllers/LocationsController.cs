using Microsoft.AspNetCore.Authorization;
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
    public async Task<ActionResult<Location>> GetNextLocation()
    {
        var location = await _locationService.QueryNextLocation();

        HttpContext.Session.SetInt32("ExpectedLocationId", location.Id);
        HttpContext.Session.SetInt32("ExpectedItemId", location.GetItemId());

        return location;
    }
    
    [Authorize(Policy = "PickerOrReacherOrRelocator")]
    [HttpPost]
    [Route("/VerifyLocation")]
    public ActionResult VerifyLocation(int locationId)
    {
        var expectedLocationId = HttpContext.Session.GetInt32("ExpectedLocationId");
        
        _locationService.ScanLocation(expectedLocationId, locationId);
        return Ok("Location verified successfully.");
    }
    
    [Authorize(Policy = "Troubleshooting")]
    [HttpGet]
    [Route("/{locationId}")]
    public async Task<Location> GetLocationById(int locationId)
        => await _locationService.QueryLocationById(locationId);
}