using Microsoft.AspNetCore.Mvc;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class RelocatingOrdersController : ControllerBase
{
    private readonly IRelocatingOrderService _relocatingOrderService;

    public RelocatingOrdersController(IRelocatingOrderService relocatingOrderService)
    {
        _relocatingOrderService = relocatingOrderService;
    }

    [HttpPatch]
    [Route("/TakeItemFromLocation")]
    public async Task TakeItemFromLocation()
    {
        var initialLocationId = HttpContext.Session.GetInt32("InitialLocationId")!.Value;
        
        await _relocatingOrderService.TakeItemFromLocation(initialLocationId);
    }
}