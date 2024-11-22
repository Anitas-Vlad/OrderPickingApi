using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Enums;
using OrderPickingSystem.Models.Requests;
using OrderPickingSystem.Models.Responses;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class UserService : IUserService
{
    private readonly OrderPickingContext _context;
    private static Regex _mailPattern;
    private static Regex _passwordPattern;
    private readonly IUserContextService _userContextService;
    private readonly IOrderService _orderService;
    private readonly IUserMapper _userMapper;

    public UserService(OrderPickingContext context, IOrderService orderService, IUserContextService userContextService,
        IUserMapper userMapper)
    {
        _context = context;
        _mailPattern = new("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
        _passwordPattern = new("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");
        _userContextService = userContextService;
        _userMapper = userMapper;
        _orderService = orderService;
    }

    public async Task<bool> CheckIfUserHasAdminRights()
        => (await _userContextService.QueryPersonalAccount()).HasRole(UserRole.Admin);

    public async Task<User> QueryUserById(int userId)
    {
        var user = await _context.Users
            .Where(user => user.Id == userId)
            .FirstOrDefaultAsync();

        if (user == null) throw new ArgumentException("User not found.");

        return user;
    }

    public async Task<User> QueryUserByUsername(string username)
    {
        var user = await _context.Users
            .Where(user => user.Username == username)
            .FirstOrDefaultAsync();

        if (user == null)
            throw new ArgumentException("User not found.");

        return user;
    }

    public async Task<List<User>> QueryAllUsers() => //TODO Admin
        await _context.Users
            .ToListAsync();

    public async Task<User> CreateUser(RegisterRequest request) //TODO Admin
    {
        await IsUsernameValid(request.Username);
        IsPasswordValid(request.Password);

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Username = request.Username,
            PasswordHash = passwordHash
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    private async Task IsUsernameValid(string username) //TODO Admin
    {
        if (await _context.Users.AnyAsync(user => user.Username == username))
            throw new ArgumentException($"the username \"{username}\" is taken.");
    }

    private static void IsPasswordValid(string userPassword) //TODO Admin
    {
        if (!_passwordPattern.IsMatch(userPassword))
            throw new ArgumentException(
                "Password must contain special characters, numbers, capital letters and be longer than 8 characters.");
    }

    public async Task<User> TakeOrder(int orderId)
    {
        var user = await _userContextService.QueryPersonalAccount();
        user.ThrowIfHasOngoingOrder();

        var order = await _orderService.QueryOrderById(orderId);
        order.ThrowIfInProgress();

        user.StartOrder(order);
        order.CurrentUserId = user.Id;
        order.OrderStatus = OrderStatus.Picking;

        _context.Orders.Update(order);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return user;
    }
}