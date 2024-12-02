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
    
    [HttpPost]
    public async Task<ActionResult<Container>> SetContainer(string containerId)
    {
        var container = await _containerService.SetContainer(containerId);
        HttpContext.Session.SetString("OngoingContainerId", container.Id);
        return container;
    }

    [HttpPost]
    [Route("/VerifyContainer/{containerId}")]
    public ActionResult VerifyContainer(string containerId)
    {
        var expectedContainerId = HttpContext.Session.GetString("OngoingContainerId");

        _containerService.ScanContainer(expectedContainerId, containerId);
        return Ok("Container verified successfully.");
    }
}