using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class PickingOrderService : OrderService
{
    private readonly OrderPickingContext _context;
    private readonly IPaletteService _paletteService;
    private readonly IUserContextService _userContextService;
    
    public PickingOrderService(OrderPickingContext context, IPaletteService paletteService,
        IUserContextService userContextService) : base(context, paletteService, userContextService)
    {
        _context = context;
        _paletteService = paletteService;
        _userContextService = userContextService;
    }

    public new async Task<List<PickingOrder>> QueryAllOrders() 
        => await _context.PickingOrders.ToListAsync();
}