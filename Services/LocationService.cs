using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class LocationService : ILocationService
{
    private readonly OrderPickingContext _context;
    private readonly IPickService _pickService;

    public LocationService(OrderPickingContext context, IPickService pickService)
    {
        _context = context;
        _pickService = pickService;
    }

    public async Task<Location> QueryLocationByID(int locationId)
    {
        var location = await _context.Locations.SingleOrDefaultAsync(location => location.Id == locationId);
        if (location == null)
            throw new ArgumentException("Location not found.");
        return location;
    }
    
    public async Task<LocationItem> QueryLocationItemByID(int locationItemId)
    {
        var locationItem = await _context.LocationItems.SingleOrDefaultAsync(location => location.Id == locationItemId);
        if (locationItem == null)
            throw new ArgumentException("Location not found.");
        return locationItem;
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
        var locationItem = await QueryLocationItemByID(request.LocationId);
        if (!locationItem.CheckIfQuantityIsEnoughToPick(request)) 
            await _pickService.CreateReplenish();

        var pick = await CreatePick(request);
    }
}