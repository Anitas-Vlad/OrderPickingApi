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

    // public Pick CreatePick(CreatePickRequest request)
    // {
    //     return new Pick
    //     {
    //         ContainerId = request.ContainerId,
    //         DateTime = DateTime.Now.ToLocalTime(),
    //         ItemId = request.ItemId,
    //         LocationId = request.LocationId,
    //         Quantity = request.Quantity,
    //         UserId = request.UserId
    //     };
    // }

    public async Task<Pick> CreatePick(CreatePickRequest request)
    {
        var pick = new Pick
        {
            ItemId = request.ItemId,
            UserId = request.UserId,
            ContainerId = request.ContainerId,
            LocationId = request.LocationId,
            Quantity = request.Quantity
        };
        _context.Picks.Add(pick);
        await _context.SaveChangesAsync();
        return pick;
    }
}