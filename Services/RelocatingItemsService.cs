using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Orders;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class RelocatingItemsService :IRelocatingItemsService
{
    private readonly ILocationService _locationService;
    private readonly OrderPickingContext _context;
    private readonly IUserContextService _userContextService;
    private readonly IRelocatingOrderService _relocatingOrderService;
    
    public RelocatingItemsService(OrderPickingContext context, IRelocatingOrderService relocatingOrderService,
        IUserContextService userContextService, ILocationService locationService)
    {
        _context = context;
        _locationService = locationService;
        _userContextService = userContextService;
        _relocatingOrderService = relocatingOrderService;
    }
    
    public async Task<Relocation> TakeItemFromLocation(int initialLocationId)
    {
        if (initialLocationId == 0)
            throw new ArgumentException("Initial location id missing.");

        var userId = _userContextService.GetUserId();
        var location = await _locationService.QueryLocationById(initialLocationId);
        var item = location.Item;

        var relocation = _relocatingOrderService.CreateRelocation(initialLocationId, userId);

        location.RemoveItem();
        item!.RemoveFromLocation();

        _context.Relocations.Update(relocation);
        _context.Locations.Update(location);
        _context.Items.Update(item);

        await _context.SaveChangesAsync();
        return relocation;
    }
    
    public async Task RelocateItemToLocation(int locationId, int relocationId) //TODO finish
    {
        var user = await _userContextService.QueryPersonalAccount();
        var order = user.CurrentOrder;
        
        if (order is not RelocatingOrder relocationOrder)
            throw new ArgumentException("Invalid Order.");
        
        var item = relocationOrder.Item;
        
        var destinationLocation = await _locationService.QueryLocationById(locationId);
        
    }
}