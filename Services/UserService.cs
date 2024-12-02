using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Enums;
using OrderPickingSystem.Models.Orders;
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
    private readonly IUserRoleMappingService _userRoleMappingService;

    public UserService(OrderPickingContext context, IOrderService orderService, IUserContextService userContextService,
        IUserMapper userMapper, IUserRoleMappingService roleMappingService)
    {
        _context = context;
        _mailPattern = new("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
        _passwordPattern = new("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");
        _userContextService = userContextService;
        _userMapper = userMapper;
        _orderService = orderService;
        _userRoleMappingService = roleMappingService;
    }

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
            .Include(user => user.UserRoles)
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

    public async Task<User> AddUserRole(int userId, UserRole role)
    {
        var user = await QueryUserById(userId);
        if (user.HasRole(role))
            throw new ArgumentException("Role already exists.");

        var userRoleMapping = _userRoleMappingService.CreateUserRoleMapping(userId, role);

        user.AddRole(userRoleMapping);

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> RemoveUserRole(int userId, UserRole role)
    {
        var user = await QueryUserById(userId);

        var urm = user.GetUserRoleMapping(role);

        if (urm == null)
            throw new ArgumentException("User does not have this role.");

        user.RemoveRole(urm);

        _context.UserRoleMappings.Remove(urm);
        _context.Users.Update(user);

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

    public async Task<Order> TakeOrder(int orderId)
    {
        var user = await _userContextService.QueryPersonalAccount();
        if (user.CurrentOrder != null)
            throw new ArgumentException("You currently have an ongoing order.");

        var order = await _orderService.QueryOrderById(orderId);
        if (order.OrderStatus == OrderStatus.InProgress)
            throw new ArgumentException("This order is taken by another worker.");
                
        if (!user.HasRole(order.RequiredRole))
            throw new ArgumentException("This user does not have the required role.");
        
        user.StartOrder(order);
        order.CurrentUserId = user.Id;
        order.OrderStatus = OrderStatus.InProgress;
        
        _context.Orders.Update(order);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        
        return order;
    }
}