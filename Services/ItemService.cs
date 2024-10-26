using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class ItemService : IItemService
{
    
    private readonly OrderPickingContext _context;

    public ItemService(OrderPickingContext context)
    {
        _context = context;
    }
    
    public async Task<Item> QueryItemById(int itemId)
    {
        var item = await _context.Items.SingleOrDefaultAsync(item => item.Id == itemId);
        if (item == null)
            throw new ArgumentException("Item not found.");
        return item;
    }
}