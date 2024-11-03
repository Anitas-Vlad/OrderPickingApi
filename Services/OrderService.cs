using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class OrderService : IOrderService
{
    private readonly OrderPickingContext _context;
    private readonly ILocationService _locationService;

    public OrderService(OrderPickingContext context, ILocationService locationService)
    {
        _context = context;
        _locationService = locationService;
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

    public Task<List<Pick>> QueryPicksByOrderId(int OrderId)
    {
        throw new NotImplementedException();
    }
}