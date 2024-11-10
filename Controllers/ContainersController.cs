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
    
    [HttpPatch]
    public async Task<ActionResult<Container>> SetContainer(string containerId)
    {
        var container = await _containerService.SetContainer(containerId);
        HttpContext.Session.SetString("OngoingContainerId", container.Id);
        return container;
    }
    
    // [HttpPost]
    // [Route("/VerifyOngoingContainer")]
    // public void VerifyOngoingContainer(string containerId)
    // {
    //     var expectedLocationId = HttpContext.Session.GetString("OngoingContainerId");
    //     
    //     if (expectedLocationId == null || expectedLocationId != containerId)
    //         throw new ArgumentException("Incorrect container scanned. Please verify and try again.");
    // }
}