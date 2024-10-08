﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Responses;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    [Route("/PersonalAccount")]
    public async Task<ActionResult<User>> GetPersonalAccount() 
        => await _userService.QueryPersonalAccount();

    [HttpGet]
    [Route("/{username}")]
    public async Task<ActionResult<UserResponse>> SearchUserProfile(string username)
        => await _userService.QueryUserProfile(username);
    
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAllUsers() 
        => await _userService.QueryAllUsers();
}