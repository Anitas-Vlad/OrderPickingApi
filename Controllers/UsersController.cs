using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers;

// [Authorize]
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

    [HttpPatch]
    [Route("/TakeOrder-{orderId}")]
    public async Task<ActionResult<User>> TakeOrder(int orderId)
        => await _userService.TakeOrder(orderId);
}