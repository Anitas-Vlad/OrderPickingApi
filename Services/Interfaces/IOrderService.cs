using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface IOrderService
{
    Task<List<Order>> QueryAllOrders();

    Task<Order> QueryOrderById(int orderId);
    Task<List<Pick>> QueryPicksByOrderId(int OrderId); //TODO
}