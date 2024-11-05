using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Enums;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class OrderService : IOrderService
{
    private readonly OrderPickingContext _context;
    private readonly IPaletteService _paletteService;

    public OrderService(OrderPickingContext context, IPaletteService paletteService)
    {
        _context = context;
        _paletteService = paletteService;
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

    // public async Task<Order> SetPalette(int PaletteId)
    // {
    //     var order = 
    //     var palette = await _paletteService.CreatePalette()
    // }
}