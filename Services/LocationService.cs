using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class LocationService : ILocationService
{
    private readonly OrderPickingContext _context;
    private readonly IPickService _pickService;
    private readonly IItemService _itemService;

    public LocationService(OrderPickingContext context, IPickService pickService, IItemService itemService)
    {
        _context = context;
        _pickService = pickService;
        _itemService = itemService;
    }

    public async Task<Location> QueryLocationById(int locationId)
    {
        var location = await _context.Locations.SingleOrDefaultAsync(location => location.Id == locationId);
        if (location == null)
            throw new ArgumentException("Location not found.");
        return location;
    }
    
    public async Task<Location> QueryNextLocation(Order order)
    {
        var pickingLocationsQueue = await QueryPickingLocationsQueue();
        var itemFound = false;

        var nextLocation = pickingLocationsQueue.Dequeue();

        while (itemFound == false)
        {
            if (nextLocation == null)
            {
                if (order.ReplenishItems.Count > 0)
                    throw new ArgumentException(
                        "There are still items to be picked. Leave the Palette in the Replenish Area.");

                throw new ArgumentException("The order is completed. You must now print the label.");
            }

            var nextLocationItem = nextLocation.Item;
            var wantedItem = order.GetItemById(nextLocationItem.LocationId);

            if (wantedItem == null)
                continue;

            if (!nextLocationItem.CheckIfQuantityIsEnoughToPick(wantedItem.Quantity))
            {
                order.EnqueueReplenishItem(wantedItem);
                _context.Orders.Update(order);
            }
            else
                itemFound = true;
        }

        return nextLocation;
    }
    
    public async Task<Queue<Location>> QueryPickingLocationsQueue()
    {
        var dictionary = await QueryPickingLocations();
        var locationsQueue = OrderPickingLocationsQueue(dictionary);
        
        return locationsQueue;
        // return OrderPickingLocationsQueue(await QueryPickingLocations());
    }
    
    public async Task<Queue<Location>> QueryReplenishLocationsQueue()
    {
        var dictionary = await QueryReplenishLocations();
        var locationsQueue = OrderPickingLocationsQueue(dictionary);

        return locationsQueue;
    }

    private Queue<Location> OrderPickingLocationsQueue(SortedDictionary<int, List<Location>> locationsDictionary)
    {
        var locationsQueue = new Queue<Location>();
        foreach (var (key, isleLocations) in locationsDictionary)
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

    // These are all the 1st floor picking locations.
    // Maybe there should be another table in the DB for the PickingLocations so that this request is not made every time an order is starte.
    private async Task<SortedDictionary<int, List<Location>>> QueryPickingLocations()
    {
        var locations = await _context.Locations.Where(location => location.Floor == 1)
            .OrderBy(location => location.Isle).ToListAsync();

        var sortedLocations = CreateIsleSortedLocationsDictionary(locations);

        return sortedLocations;
    }
    
    // These are all the locations from where the reach truckers have to take Items to replenish the Picking locations on the first floor.
    private async Task<SortedDictionary<int, List<Location>>> QueryReplenishLocations()
    {
        var locations = await _context.Locations.Where(location => location.Floor != 1)
            .OrderBy(location => location.Isle).ToListAsync();

        var sortedLocations = CreateIsleSortedLocationsDictionary(locations);

        return sortedLocations;
    }


    private SortedDictionary<int, List<Location>> CreateIsleSortedLocationsDictionary(List<Location> locations)
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