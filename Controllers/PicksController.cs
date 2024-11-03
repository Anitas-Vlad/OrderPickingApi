using Microsoft.AspNetCore.Mvc;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers;

[ApiController]
[Route("{controller}")]
public class PicksController : ControllerBase
{
    private readonly IPickService _pickService;

    public PicksController(IPickService pickService)
    {
        _pickService = pickService;
    }

    public async Task<ActionResult<List<Pick>>> GetPicksByOrderId(int orderId)
        => await _pickService.QueryPicksByOrderId(orderId);
    
    
}