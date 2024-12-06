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
    
    
}