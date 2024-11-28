using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Orders;

namespace OrderPickingSystem.Services.Interfaces;

public interface IOrderService
{
    Task<List<Order>> QueryAllOrders();

    Task<Order> QueryOrderById(int orderId);
}