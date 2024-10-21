using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class PickService : IPickService
{
    private readonly OrderPickingContext _context;

    public PickService(OrderPickingContext context)
    {
        _context = context;
    }

    public async Task<List<Pick>> QueryAllPicks() 
        => await _context.Picks.ToListAsync();
    
    public Task<List<Pick>> QueryPicksByContainerId(int containerId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Pick>> QueryPicksByOrderId(int orderId)
    {
        throw new NotImplementedException();
    }

    public Task<Item> CreateReplenish()
    {
        throw new NotImplementedException();
    }
}