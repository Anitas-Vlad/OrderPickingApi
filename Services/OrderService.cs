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

    public async Task<List<Order>> QueryAllOrders() => await _context.Orders.Order().ToListAsync();

    public async Task<Order> QueryOrderById(int orderId)
    {
        var order = await _context.Orders.SingleOrDefaultAsync(order => order.Id == orderId);
        if (order == null)
            throw new ArgumentException("Order not found.");
        return order;
    }

    public async Task<Location> QueryNextLocation(int orderId) 
    {
        // Search for the item with the locationId the closest to you (ordered).
        
        var order = await QueryOrderById(orderId);
        // Order has more items

        var items = order.Items;
        // Each item has a LocationId
        
        //TODO Method to search and select only the Pickers Locations. (first Isle)
        var locations = new List<Location>();
        
        
        
        throw new NotImplementedException();
    }
    
    public async Task<List<Location>> QueryPickingLocationsForOrder(int orderId)
        //TODO Maybe there should be another table in the DB for the PickingLocations so that this request is not made every time an order is starte.
    {
        var order = await QueryOrderById(orderId);

        //TODO Delete the order from here and create another method specifically for ordering
        var pickingLocations = await _context.Locations.Where(location => location.Floor == 1)
            .OrderBy(location => location.Isle).
            ToListAsync();

        return pickingLocations;
    }

    public Task<List<Location>> OrganizePickingLocations(List<Location> locations)
    {
        throw new NotImplementedException();
    }
}