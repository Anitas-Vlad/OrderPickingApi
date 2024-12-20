﻿using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models.Orders;
using OrderPickingSystem.Models.TaskRequests;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services.OrderServices;

public class ReachingOrderService : OrderService, IReachingOrderService
{
    private readonly OrderPickingContext _context;
    private readonly IPaletteService _paletteService;
    private readonly IUserContextService _userContextService;
    private readonly ILocationService _locationService;
    private readonly IItemService _itemService;
    
    public ReachingOrderService(OrderPickingContext context, IPaletteService paletteService,
        ILocationService locationService, IUserContextService userContextService) : base(context, userContextService)
    {
        _context = context;
        _paletteService = paletteService;
        _userContextService = userContextService;
        _locationService = locationService;
    }
    
    private async Task<List<PickingOrder>> QueryOrdersByReplenishItemId(int itemId) =>
        await _context.PickingOrders
            .Where(order => order.ReplenishedRequestedItems.Any(item => item.ItemId == itemId))
            .ToListAsync();
    
    public async Task UpdateOrdersAfterReplenishmentByItemId(int itemId)
    {
        var orders = await QueryOrdersByReplenishItemId(itemId);

        foreach (var order in orders)
        {
            order.UpdateAfterReplenishment(itemId);
            _context.Orders.Update(order);
        }
    
        await _context.SaveChangesAsync(); //TODO maybe this is not needed here, but where it'll be used
    }
}