using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Requests;
using OrderPickingSystem.Models.Responses;

namespace OrderPickingSystem.Services.Interfaces;

public interface IUserService
{
    Task<User> QueryUserById(int userId);
    Task<UserResponse> QueryUserProfile(string username);
    Task<User> QueryPersonalAccount();
    Task<List<User>> QueryAllUsers();
    Task<User?> QueryUserByEmail(string userEmail);
    Task<User> CreateUser(RegisterRequest request);
    Task<Order> QueryOngoingOrder();
}