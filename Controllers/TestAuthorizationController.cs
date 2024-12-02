using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers;

[Authorize]
[ApiController]
[Route("/[controller]")]
public class TestAuthorizationController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserContextService _userContextService;

    public TestAuthorizationController(IUserService userService, IUserContextService userContextService)
    {
        _userService = userService;
        _userContextService = userContextService;
    }
    
    [HttpGet]
    [Route("/Test/PersonalAccount")]
    public async Task<ActionResult<User>> TestGetPersonalAccount()
        => await _userContextService.QueryPersonalAccount();
}