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

    //TODO AdminApi
    public async Task<List<Order>> QueryAllOrders()
        => await _context.Orders.Order().ToListAsync();

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

            palette.OrderId = order.Id;
            order.SetOngoingPalette(palette);

            _context.Palettes.Update(palette);
        }
        else
        {
            optionalPalette.OrderId = order.Id;
            order.SetOngoingPalette(optionalPalette);
            
            _context.Palettes.Update(optionalPalette);
        }

        _context.Orders.Update(order);
        await _context.SaveChangesAsync();

        return order;
    }
}