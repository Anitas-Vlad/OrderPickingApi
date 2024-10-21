using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class LocationService : ILocationService
{
    private readonly OrderPickingContext _context;
    private readonly IPickService _pickService;
    private readonly IItemService _itemService;

    public LocationService(OrderPickingContext context, IPickService pickService, IItemService itemService)
    {
        _context = context;
        _pickService = pickService;
        _itemService = itemService;
    }

    public async Task<Location> QueryLocationById(int locationId)
    {
        var location = await _context.Locations.SingleOrDefaultAsync(location => location.Id == locationId);
        if (location == null)
            throw new ArgumentException("Location not found.");
        return location;
    }

    public async Task<Pick> CreatePick(CreatePickRequest request) //TODO Maybe don t need this method
    {
        var pick = new Pick
        {
            LocationId = request.LocationId,
            ItemId = request.LocationId,
            UserId = request.UserId,
            ContainerId = request.ContainerId
        };

        await _context.Picks.AddAsync(pick);
        return pick;
    }

    public async Task PickFromLocation(CreatePickRequest request)
    {
        var locationItem = await _itemService.QueryItemByID(request.ItemId);
        if (!locationItem.CheckIfQuantityIsEnoughToPick(request)) 
            await _pickService.CreateReplenish();

        var pick = await CreatePick(request);
    }
}