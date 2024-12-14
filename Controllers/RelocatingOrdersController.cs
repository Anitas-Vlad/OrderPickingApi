using Microsoft.AspNetCore.Mvc;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Orders;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class RelocatingOrdersController : ControllerBase
{
    private readonly IRelocatingOrderService _relocatingOrderService;
    private readonly IRelocatingItemsService _relocatingItemsService;

    public RelocatingOrdersController(IRelocatingOrderService relocatingOrderService,
        IRelocatingItemsService relocatingItemsService)
    {
        _relocatingOrderService = relocatingOrderService;
        _relocatingItemsService = relocatingItemsService;
    }

    [HttpGet]
    [Route("/RelocatingOrder-{orderId}")]
    public async Task<ActionResult<RelocatingOrder>> GetRelocatingOrderById(int orderId)
        => await _relocatingOrderService.QueryRelocatingOrderById(orderId);

    [HttpGet]
    public async Task<ActionResult<List<RelocatingOrder>>> GetRelocatingOrders()
        => await _relocatingOrderService.QueryRelocatingOrders();

    [HttpPatch]
    [Route("/TakeItemFromLocation")]
    public async Task TakeItemFromLocation()
    {
        var initialLocationId = HttpContext.Session.GetInt32("InitialLocationId")!.Value;

        var relocation = await _relocatingItemsService.TakeItemFromLocation(initialLocationId);

        HttpContext.Session.SetInt32("OngoingRelocationId", relocation.Id);
        HttpContext.Session.SetInt32("DestinationLocationId", relocation.Id);
    }

    [HttpPatch]
    [Route("/CompleteRelocation")]
    public async Task<Relocation> CompleteRelocation()
    {
        var relocationId = HttpContext.Session.GetInt32("OngoingRelocationId")!.Value;
        var destinationLocationId = HttpContext.Session.GetInt32("DestinationLocationId")!.Value;

        if (relocationId == 0)
            throw new ArgumentException("Invalid relocation id");
        
        if (destinationLocationId == 0 )
            throw new ArgumentException("Invalid destination location id");
        
        return await _relocatingItemsService.CompleteRelocation(relocationId, destinationLocationId);
    }

}