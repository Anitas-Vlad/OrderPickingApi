using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Requests;
using OrderPickingSystem.Models.Responses;

namespace OrderPickingSystem.Services.Interfaces;

public interface IUserService
{
    Task<User> QueryUserById(int userId);
    Task<List<User>> QueryAllUsers();
    Task<User> QueryUserByUsername(string username);
    Task<User> CreateUser(RegisterRequest request);
    Task<User> TakeOrder(int orderId);
}