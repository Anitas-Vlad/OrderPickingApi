using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

    public async Task<SortedDictionary<int, List<Location>>> QueryPickingLocationsForOrder(int orderId)
        //TODO Maybe there should be another table in the DB for the PickingLocations so that this request is not made every time an order is starte.
    {
        var order = await QueryOrderById(orderId);

        var locations = await _context.Locations.Where(location => location.Floor == 1)
            .OrderBy(location => location.Isle).ToListAsync();

        var sortedLocations = createIsleSortedLocationsDictionary(locations);

        return sortedLocations;
    }

    private Queue<Location> CreateLocationsQueue(SortedDictionary<int, List<Location>> dictionary)
    {
        var locationsQueue = new Queue<Location>();

        foreach (var (key, isleLocations) in dictionary)
        {
            if (key % 2 == 0)
                isleLocations.OrderBy(location => location.Number);
            else
                isleLocations.OrderByDescending(location => location.Number);

            foreach (var isleLocation in isleLocations)
            {
                locationsQueue.Enqueue(isleLocation);
            }
        }

        return locationsQueue;
    }

    private SortedDictionary<int, List<Location>> createIsleSortedLocationsDictionary(List<Location> locations)
    {
        var orderedLocations = new SortedDictionary<int, List<Location>>();

        foreach (var location in locations)
        {
            if (!orderedLocations.TryGetValue(location.Isle, out var value))
                orderedLocations.Add(location.Isle, new List<Location> { location });
            else
                value.Add(location);
        }

        return orderedLocations;
    }
}