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

    public async Task<List<Order>> QueryAllOrders() //TODO Override after breaking this service into Reach/Pick Service
        => await _context.Orders.ToListAsync();

    private async Task<List<PickingOrder>> QueryOrdersByReplenishItemId(int itemId) =>
        await _context.PickingOrders
            .Where(order => order.ReplenishedRequestedItems.Any(item => item.ItemId == itemId))
            .ToListAsync();

    public async void UpdateOrdersByReplenishedItemId(int itemId)
    {
        var orders = await QueryOrdersByReplenishItemId(itemId);

        foreach (var order in orders)
        {
            order.UpdateAfterReplenishment(itemId);
            _context.Orders.Update(order);
        }

        await _context.SaveChangesAsync(); //TODO maybe this is not needed here, but where it'll be used
    }

    public async Task<Order> QueryOrderById(int orderId)
    {
        var order = await _context.Orders.SingleOrDefaultAsync(order => order.Id == orderId);
        if (order == null)
            throw new ArgumentException("Order not found.");
        return order;
    }
}