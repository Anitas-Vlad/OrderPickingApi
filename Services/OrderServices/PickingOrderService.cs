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

    public new async Task<List<PickingOrder>> QueryAllOrders() 
        => await _context.PickingOrders.ToListAsync();

    public Task<List<Pick>> QueryPicksByOrderId(int orderId)
    {
        throw new NotImplementedException();
    }
}