using Microsoft.AspNetCore.Mvc;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Requests;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class PicksController : ControllerBase
{
    private readonly IPickService _pickService;

    public PicksController(IPickService pickService)
    {
        _pickService = pickService;
    }

    [HttpGet]
    [Route("/picks-{orderId}")]
    public async Task<ActionResult<List<Pick>>> GetPicksByOrderId(int orderId)
        => await _pickService.QueryPicksByOrderId(orderId);

    [HttpPatch]
    public async Task CompletePick() => await _pickService.CompletePick();

}