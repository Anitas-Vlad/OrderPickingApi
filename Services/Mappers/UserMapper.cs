using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Responses;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services.Mappers;

public class UserMapper : IUserMapper
{
    public UserResponse Map(User user)
        => new()
        {
            Id = user.Id,
            UserName = user.Username
        };

    public List<UserResponse> Map(List<User> users) 
        => users.Select(user => Map(user)).ToList();
}