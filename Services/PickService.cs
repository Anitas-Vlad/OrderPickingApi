using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Requests;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class PickService : IPickService
{
    private readonly OrderPickingContext _context;
    private readonly IItemService _itemService;

    public PickService(OrderPickingContext context, IItemService itemService)
    {
        _context = context;
        _itemService = itemService;
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

    public Task<Pick> CreatePick()
    {
        throw new NotImplementedException();
    }

    public Task PickFromLocation(PickRequest request)
    {
        throw new NotImplementedException();
    }
}