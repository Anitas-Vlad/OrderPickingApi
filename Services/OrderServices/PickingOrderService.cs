using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Orders;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services.OrderServices;

public class PickingOrderService : OrderService, IPickingOrderService
{
    private readonly OrderPickingContext _context;
    private readonly IPaletteService _paletteService;
    private readonly IUserContextService _userContextService;
    
    public PickingOrderService(OrderPickingContext context, IPaletteService paletteService,
        IUserContextService userContextService) : base(context, userContextService)
    {
        _context = context;
        _paletteService = paletteService;
        _userContextService = userContextService;
    }

    public async Task<PickingOrder> QueryPickingOrderById(int orderId)
    {
        var order = await _context.PickingOrders
            .Include(order => order.Picks)
            .FirstOrDefaultAsync(order => order.Id == orderId);

        if (order == null)
            throw new ArgumentException("Order not found.");

        return order;
    }

    public new async Task<List<PickingOrder>> QueryAllPickingOrders() 
        => await _context.PickingOrders.ToListAsync();

    public async Task<List<Pick>> QueryPicksByOrderId(int orderId) 
        => (await QueryPickingOrderById(orderId)).Picks;
}