using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Orders;

namespace OrderPickingSystem.Services.Interfaces;

public interface IUserContextService
{
    int GetUserId();
    Task<User> QueryPersonalAccount();
    Task<Order> QueryOngoingOrder();
}