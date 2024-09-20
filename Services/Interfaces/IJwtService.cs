using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface IJwtService
{
    string CreateToken(User user);
}