using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface IOrderService
{
    Task<List<Order>> QueryAllOrders();

    Task<Order> QueryOrderById(int orderId);

    //TODO Admin Api - Task<List<Pick>> QueryPicksByOrderId(int OrderId);
    Task<Location> QueryNextLocation(Order order);
}