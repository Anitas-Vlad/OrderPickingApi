using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface IPickService
{
    Task<List<Pick>> QueryPicksByContainerId(int containerId);
    Task<List<Pick>> QueryPicksByOrderId(int orderId);
    Task<Item> CreateReplenish();
}