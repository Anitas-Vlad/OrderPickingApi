using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Orders;
using OrderPickingSystem.Models.TaskRequests;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class RelocatingItemsService : IRelocatingItemsService
{
    private readonly ILocationService _locationService;
    private readonly OrderPickingContext _context;
    private readonly IUserContextService _userContextService;
    private readonly IRelocatingOrderService _relocatingOrderService;
    private readonly IItemService _itemService;

    public RelocatingItemsService(OrderPickingContext context, IRelocatingOrderService relocatingOrderService,
        IUserContextService userContextService, ILocationService locationService, IItemService itemService)
    {
        _context = context;
        _locationService = locationService;
        _userContextService = userContextService;
        _relocatingOrderService = relocatingOrderService;
        _itemService = itemService;
    }

    public async Task<RelocateItemRequest> QueryNextRelocationRequest()
    {
        var order = await _userContextService.QueryOngoingOrder();

        return order switch
        {
            ReachingOrder reachingOrder => HandleQueryNextReachItemRequest(reachingOrder),
            RelocatingOrder relocatingOrder => await HandleQueryNextRelocateItemRequest(relocatingOrder)
        };
    }

    private RelocateItemRequest HandleQueryNextReachItemRequest(ReachingOrder order)
        => order.Request;

    private async Task<RelocateItemRequest> HandleQueryNextRelocateItemRequest(RelocatingOrder order)
    {
        if (order.OngoingRequest != null)
            return order.OngoingRequest;
        
        var ongoingRelocateItemRequest = order.Requests.Dequeue();
        
        if (ongoingRelocateItemRequest is null)
            throw new ArgumentException("There are no more items to be relocated in this order.");

        order.OngoingRequest = ongoingRelocateItemRequest;

        _context.RelocatingOrders.Update(order);
        await _context.SaveChangesAsync();

        return ongoingRelocateItemRequest;
    }

    public async Task<Relocation> TakeItemFromLocation(int initialLocationId)
    {
        if (initialLocationId == 0)
            throw new ArgumentException("Invalid location id.");

        var userId = _userContextService.GetUserId();
        var location = await _locationService.QueryLocationById(initialLocationId);

        var item = location.Item;
        if (item == null)
            throw new ArgumentException("Initial location does not contain any item.");

        var relocation = _relocatingOrderService.CreateRelocation(initialLocationId, userId, item.Id);

        location.RemoveItem();
        item.RemoveFromLocation();

        _context.Relocations.Update(relocation);
        _context.Locations.Update(location);
        _context.Items.Update(item);

        await _context.SaveChangesAsync();
        return relocation;
    }

    public async Task<Relocation> CompleteRelocation(int relocationId, int destinationLocationId)
    {
        var user = await _userContextService.QueryPersonalAccount();

        var order = user.CurrentOrder;
        if (order == null)
            throw new ArgumentException("You have no ongoing order.");

        var relocation = await _context.Relocations
            .FirstOrDefaultAsync(relocation => relocation.Id == relocationId);

        if (relocation == null)
            throw new ArgumentException("Invalid relocation id.");

        var item = await _itemService.QueryItemById(relocation.ItemId);

        var destinationLocation = await _locationService.QueryLocationById(destinationLocationId);
        if (destinationLocation.Item != null)
            throw new ArgumentException("This location already contains an item.");

        destinationLocation.Item = item;
        item.RelocateToLocation(destinationLocation);
        relocation.DestinationLocationId = destinationLocationId;
        relocation.DateTime = DateTime.Now;

        _context.Locations.Update(destinationLocation);
        _context.Relocations.Update(relocation);
        _context.Items.Update(item);

        await _context.SaveChangesAsync();

        return relocation;
    }
}