using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers.Administration;

[ApiController]
[Authorize(Roles = "Admin")]
public class AdministrationContainersController
{
    private readonly IContainerService _containerService;

    public AdministrationContainersController(IContainerService containerService)
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