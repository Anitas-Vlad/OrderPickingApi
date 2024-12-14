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

    public async Task<List<RelocatingOrder>> QueryRelocatingOrders()
        => await _context.RelocatingOrders.ToListAsync();

    public Relocation CreateRelocation(int initialLocationId, int userId, int itemId) //TODO Maybe I do this only at the end.
    {
        var relocation = new Relocation()
        {
            InitialLocationId = initialLocationId,
            UserId = userId,
            ItemId = itemId
        };

        _context.Relocations.Add(relocation);

        return relocation;
    }
}