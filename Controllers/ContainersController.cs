using Microsoft.AspNetCore.Mvc;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class ContainersController : ControllerBase
{
    private readonly IContainerService _containerService;

    public ContainersController(IContainerService containerService)
    {
        _containerService = containerService;
    }

    [HttpGet]
    [Route("/{containerId}")]
    public async Task<ActionResult<Container>> GetContainerById(string containerId)
    {
        var container = await _containerService.QueryContainerById(containerId);
        if (container == null)
            throw new ArgumentException("Container not found.");
        return container;
    }
}