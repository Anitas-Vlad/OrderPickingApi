using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers.Administration;

[ApiController]
// [Authorize(Roles = "Admin")]
[Route("[controller]")]
public class AdministrationUsersController : ControllerBase
{
    private readonly IUserService _userService;

    public AdministrationUsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
        => await _userService.QueryAllUsers();
}