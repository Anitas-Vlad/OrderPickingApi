using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Orders;

namespace OrderPickingSystem.Services.Interfaces;

public interface IPickingOrderService
{
    Task<List<PickingOrder>> QueryAllOrders();
    Task<List<Pick>> QueryPicksByOrderId(int orderId);

}