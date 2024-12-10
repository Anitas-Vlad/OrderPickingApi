using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Enums;
using OrderPickingSystem.Models.Orders;
using OrderPickingSystem.Models.Requests;
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
    public async Task<ActionResult<List<User>>> GetAllUsers()
        => await _userService.QueryAllUsers();

    [HttpPatch]
    [Route("/TakeOrder-{orderId}")]
    public async Task<ActionResult<Order>> TakeOrder(int orderId)
    {
        var order = await _userService.TakeOrder(orderId);
        switch (order) //TODO Write this in different method. This one only takes the order. The other one sets locations
        {
            case ReachingOrder reachingOrder:
                var request = reachingOrder.Request;
                HttpContext.Session.SetInt32("InitialLocationId", request.InitialLocationId);
                HttpContext.Session.SetInt32("DestinationLocationId", request.DestinationLocationId);
                break;
            
            case RelocatingOrder relocatingOrder: //TODO
                break;
        }

        return order;
    }
}