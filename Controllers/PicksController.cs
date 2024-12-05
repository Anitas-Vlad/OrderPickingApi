using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Policy = "Troubleshooting")]
    [HttpGet]
    [Route("/picks-{orderId}")]
    public async Task<ActionResult<List<Pick>>> GetPicksByOrderId(int orderId)
        => await _pickService.QueryPicksByOrderId(orderId);

    [Authorize(Policy = "Troubleshooting")]
    [HttpGet]
    [Route("/picks-from-container-{containerId}")]
    public async Task<ActionResult<List<Pick>>> GetPicksFromContainer(string containerId)
        => await _pickService.QueryPicksByContainerId(containerId);
    
    [HttpPatch]
    public async Task CompletePick()
    {
        await _pickService.CompletePick();
    }
}