using Microsoft.AspNetCore.Mvc;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class ReachingOrdersController : ControllerBase
{
    private readonly IReachingOrderService _reachingOrderService;

    public ReachingOrdersController(IReachingOrderService reachingOrderService)
    {
        _reachingOrderService = reachingOrderService;
    }
    
    [HttpPatch]
    [Route("/UpdateOrdersAfterReplenishment")]
    public async Task UpdateOrdersByReplenishedItemId(int itemId)
        => await _reachingOrderService.UpdateOrdersAfterReplenishmentByItemId(itemId);
    
}