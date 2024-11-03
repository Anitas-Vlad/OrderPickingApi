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
    private readonly IUserMapper _userMapper;

    public UserService(OrderPickingContext context, IUserContextService userContextService, IUserMapper userMapper)
    {
        _context = context;
        _mailPattern = new("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
        _passwordPattern = new("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");
        _userContextService = userContextService;
        _userMapper = userMapper;
    }

    public async Task<bool> CheckIfUserHasAdminRights()
        => (await QueryPersonalAccount()).UserRights == UserRights.Admin;

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

    public async Task<UserResponse> QueryUserProfile(string username)
    {
        var user = await QueryUserByUsername(username);
        var userResponse = _userMapper.Map(user);
        return userResponse;
    }

    public async Task<User> QueryPersonalAccount()
    {
        var userId = _userContextService.GetUserId();

        var user = await _context.Users
            .Where(user => user.Id == userId)
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
}