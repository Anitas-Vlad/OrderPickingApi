using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrderPickingSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class JwtController : ControllerBase
{
    // [Authorize]
    [HttpGet("testIdFromClaims")]
    public IActionResult TestEndpoint()
    {
        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
        return Ok($"Role in Token: {roleClaim}");
    }
    
    // [Authorize]
    [HttpGet("testNameFromClaims")]
    public IActionResult TestEndpointName()
    {
        var userName = User.Identity?.Name;
        return Ok($"Name: , {userName}");
    }
}