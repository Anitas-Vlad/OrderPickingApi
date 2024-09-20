using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Responses;

namespace OrderPickingSystem.Services.Interfaces;

public interface IUserMapper
{
    UserResponse Map(User user);
    List<UserResponse> Map(List<User> users);
}