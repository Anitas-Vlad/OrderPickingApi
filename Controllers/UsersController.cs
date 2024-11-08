using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers;

// [Authorize(Roles = "Admin")]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserContextService _userContextService;

    public UsersController(IUserService userService, IUserContextService userContextService)
    {
        _userService = userService;
        _userContextService = userContextService;
    }

    [HttpGet]
    [Route("/PersonalAccount")]
    public async Task<ActionResult<User>> GetPersonalAccount() 
        => await _userContextService.QueryPersonalAccount();

    [HttpGet]
    // [AllowAnonymous]
    public async Task<ActionResult<List<User>>> GetAllUsers()
        => await _userService.QueryAllUsers();
}