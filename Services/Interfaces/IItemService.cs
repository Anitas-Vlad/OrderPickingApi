using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface IItemService
{
    Task<Item> QueryItemById(int itemId);
}