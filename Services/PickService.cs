using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
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

    public Task PickFromLocation(CreatePickRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Pick> CreatePick(CreatePickRequest request)
        // Maybe don t need this method
    {
        var pick = new Pick
        {
            LocationId = request.LocationId,
            ItemId = request.LocationId,
            UserId = request.UserId,
            ContainerId = request.ContainerId
        };
        //TODO much more logic to do here
        
        await _context.Picks.AddAsync(pick);
        return pick;
    }

    
}