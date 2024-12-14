using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Orders;
using OrderPickingSystem.Models.TaskRequests;

namespace OrderPickingSystem.Services.Interfaces;

public interface IRelocatingOrderService
{
    Task<RelocatingOrder> QueryRelocatingOrderById(int orderId);
    Task<List<RelocatingOrder>> QueryRelocatingOrders();
    Relocation CreateRelocation(int initialLocationId, int userId, int itemId);
    // Task TakeItemFromLocation(int  initialLocationId);
}