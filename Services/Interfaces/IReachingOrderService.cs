namespace OrderPickingSystem.Services.Interfaces;

public interface IReachingOrderService
{
    Task UpdateOrdersAfterReplenishmentByItemId(int itemId);
}