using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface IItemService
{
    Task<Item> QueryItemById(int itemId);
    void ScanItem(int? expectedItemId, int itemId);
}