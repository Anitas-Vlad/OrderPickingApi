using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Orders;
using OrderPickingSystem.Models.TaskRequests;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services.OrderServices;

public class RelocatingOrderService : OrderService, IRelocatingOrderService
{
    private readonly ILocationService _locationService;
    private readonly OrderPickingContext _context;
    private readonly IUserContextService _userContextService;

    public RelocatingOrderService(OrderPickingContext context,
        IUserContextService userContextService, ILocationService locationService) : base(context, userContextService)
    {
        _context = context;
        _locationService = locationService;
        _userContextService = userContextService;
    }

    public async Task<RelocatingOrder> QueryRelocatingOrderById(int orderId)
    {
        var order = await _context.RelocatingOrders.FirstOrDefaultAsync(order => order.Id == orderId);
        if (order == null)
            throw new ArgumentException("No order found.");

        return order;
    }

    public async Task<List<RelocatingOrder>> GetRelocatingOrders()
        => await _context.RelocatingOrders.ToListAsync();

    private Relocation CreateRelocation(int initialLocationId, int userId) //TODO Maybe I do this only at the end.
    {
        var relocation = new Relocation()
        {
            InitialLocationId = initialLocationId,
            UserId = userId
        };

        _context.Relocations.Add(relocation);

        return relocation;
    }

    public async Task TakeItemFromLocation(int initialLocationId)
    {
        if (initialLocationId == 0)
            throw new ArgumentException("Initial location id missing.");

        var userId = _userContextService.GetUserId();
        var location = await _locationService.QueryLocationById(initialLocationId);
        var item = location.Item;

        var relocation = CreateRelocation(initialLocationId, userId);

        location.RemoveItem();
        item!.RemoveFromLocation();

        _context.Relocations.Update(relocation);
        _context.Locations.Update(location);
        _context.Items.Update(item);

        await _context.SaveChangesAsync();
    }

    // public async Task RelocateItemToLocation(int locationId) //TODO finish
    // {
    //     var order = await _userContextService.QueryOngoingOrder();
    //     
    //     if (order is not RelocatingOrder relocationOrder)
    //         throw new ArgumentException("Invalid Order.");
    //     
    //     var user = await _userContextService.QueryPersonalAccount();
    //     
    //     var item = relocationOrder.Item;
    // }
}