using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface IPickingOrderService
{
    Task<List<PickingOrder>> QueryAllOrders();
}