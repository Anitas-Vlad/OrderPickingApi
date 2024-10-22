using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface IOrderService
{
    Task<List<Order>> QueryAllOrders();
    Task<Order> QueryOrderById(int orderId);
    // Task<List<Pick>> QueryPicksByOrderId(int OrderId);//TODO Admin Api
    Task<Location> QueryNextLocation(int orderId);
    Task<List<Location>> QueryPickingLocationsForOrder(int orderId);
    Task<List<Location>> OrganizePickingLocations(List<Location> locations);
    

}