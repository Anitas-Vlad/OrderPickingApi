using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class OrderService : IOrderService
{
    private readonly OrderPickingContext _context;
    private readonly IPaletteService _paletteService;
    private readonly IUserContextService _userContextService;

    public OrderService(OrderPickingContext context, IPaletteService paletteService,
        IUserContextService userContextService)
    {
        _context = context;
        _paletteService = paletteService;
        _userContextService = userContextService;
    }

    public async Task<List<Order>> QueryAllOrders()
        => await _context.Orders.ToListAsync();

    private async Task<List<Order>> QueryOrdersByReplenishItemId(int itemId) =>
        await _context.Orders
            .Where(order => order.ReplenishedRequestedItems.Any(item => item.ItemId == itemId))
            .ToListAsync();

    public async void UpdateOrdersByReplenishItemId(int itemId)
    {
        var orders = await QueryOrdersByReplenishItemId(itemId);

        foreach (var order in orders)
        {
            
        }
    }

    public async Task<Order> QueryOrderById(int orderId)
    {
        var order = await _context.Orders.SingleOrDefaultAsync(order => order.Id == orderId);
        if (order == null)
            throw new ArgumentException("Order not found.");
        return order;
    }

    public async Task<Order> SetPalette(string paletteId)
    {
        var order = await _userContextService.QueryOngoingOrder();
        var optionalPalette = await _paletteService.GetOptionalPaletteInProgress(paletteId, order.Id);
        
        if (optionalPalette == null)
        {
            var palette = await _paletteService.CreatePalette(paletteId);

            await SetPaletteToOrder(palette, order);
        }
        else
        {
            await SetPaletteToOrder(optionalPalette, order);
        }

        await _context.SaveChangesAsync();

        return order;
    }

    private async Task SetPaletteToOrder(Palette palette, Order order)
    {
        palette.OrderId = order.Id;
        order.SetOngoingPalette(palette);

        _context.Orders.Update(order);
        _context.Palettes.Update(palette);
        await _context.SaveChangesAsync();
    }
}