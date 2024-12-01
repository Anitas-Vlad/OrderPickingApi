using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models.Orders;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services.OrderServices;

public class OrderService: IOrderService
{
    private readonly OrderPickingContext _context;
    private readonly IUserContextService _userContextService;

    public OrderService(OrderPickingContext context,
        IUserContextService userContextService)
    {
        _context = context;
        _userContextService = userContextService;
    }

    public async Task<List<Order>> QueryAllOrders() => await _context.Orders.ToListAsync();

    public async Task<Order> QueryOrderById(int orderId)
    {
        var order = await _context.Orders
            .SingleOrDefaultAsync(order => order.Id == orderId);
        
        if (order == null)
            throw new ArgumentException("Order not found.");
        
        return order;
    }
}