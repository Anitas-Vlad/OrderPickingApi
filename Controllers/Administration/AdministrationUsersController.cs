using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Enums;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers.Administration;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("[controller]")]
public class AdministrationUsersController : ControllerBase
{
    private readonly IUserService _userService;

    public AdministrationUsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Picker")]
    public async Task<ActionResult<List<User>>> GetUsers()
        => await _userService.QueryAllUsers();
    
    [HttpPut]
    [Route("/AddUserRole/User-{userId}/Role-{role}")]
    public async Task<User> AddRoleToUser(int userId, UserRole role)
        => await _userService.AddUserRole(userId, role);

    [HttpDelete]
    [Route("/RemoveUserRole/User-{userId}/Role-{role}")]
    public async Task<User> RemoveUserRole(int userId, UserRole role)
        => await _userService.RemoveUserRole(userId, role);
}