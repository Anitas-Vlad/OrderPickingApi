using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services;

public interface IItemService
{
    Task<Item> QueryItemByID(int itemId);
}