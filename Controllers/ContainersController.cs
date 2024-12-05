using Microsoft.AspNetCore.Authorization;
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
    
    [Authorize(Policy = "Picker")]
    [HttpPost]
    [Route("/SetContainer/{containerId}")]
    public async Task<ActionResult<Container>> SetContainer(string containerId)
    {
        var container = await _containerService.SetContainer(containerId);
        HttpContext.Session.SetString("OngoingContainerId", container.Id);
        return container;
    }

    [Authorize(Policy = "Picker")]
    [HttpPost]
    [Route("/VerifyContainer/{containerId}")]
    public ActionResult VerifyContainer(string containerId)
    {
        var expectedContainerId = HttpContext.Session.GetString("OngoingContainerId");

        _containerService.ScanContainer(expectedContainerId, containerId);
        return Ok("Container verified successfully.");
    }
    
    [Authorize(Policy = "Troubleshooting")]
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