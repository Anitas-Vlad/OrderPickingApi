using System.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Orders;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly OrderPickingContext _context;


    public UserContextService(IHttpContextAccessor httpContextAccessor, OrderPickingContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    public int GetUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            throw new InvalidOperationException("Invalid or missing user ID claim.");

        return userId;
    }

    public async Task<User> QueryPersonalAccount()
    {
        var userId = GetUserId();

        var user = await _context.Users
            .Where(user => user.Id == userId)
            .Include(user => user.CurrentOrder) //TODO may need to include every(needed) info from order
            .FirstOrDefaultAsync();

        if (user == null) throw new ArgumentException("You are not logged in.");

        return user;
    }

    public async Task<Order> QueryOngoingOrder()
    {
        var optionalOrder = (await QueryPersonalAccount()).CurrentOrder;
        if (optionalOrder == null)
            throw new ArgumentException("There is no ongoing order, please select one.");

        return optionalOrder;
    }
}