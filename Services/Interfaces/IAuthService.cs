using OrderPickingSystem.Models.Requests;

namespace OrderPickingSystem.Services.Interfaces;

public interface IAuthService
{
    Task<string> Login(LoginRequest request);
}