using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class LocationService : ILocationService
{
    private readonly OrderPickingContext _context;
    private readonly IUserService _userService;

    public LocationService(OrderPickingContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<Location> QueryLocationById(int locationId)
    {
        var location = await _context.Locations.SingleOrDefaultAsync(location => location.Id == locationId);
        if (location == null)
            throw new ArgumentException("Location not found.");
        return location;
    }

    public async Task<Location> QueryNextLocation()
    {
        var order = await _userService.QueryOngoingOrder();

        var pickingLocationsQueue = await GetLocationsQueue();
        var itemFound = false;

        var nextLocation = pickingLocationsQueue.Dequeue();

        while (itemFound == false)
        {
            if (nextLocation == null)
            {
                if (order.ReplenishedRequestedItems.Count > 0)
                    throw new ArgumentException(
                        "There are still items to be picked. Leave the Palette in the Replenish Area.");

                throw new ArgumentException("The order is completed. You must now print the label.");
            }

            var nextLocationItem = nextLocation.Item;
            var pickRequest = order.GetItemById(nextLocationItem.LocationId);

            if (pickRequest == null)
                continue;

            if (!nextLocationItem.CheckIfQuantityIsEnoughToPick(pickRequest.Quantity))
            {
                order.EnqueueReplenishItem(pickRequest);
                _context.Orders.Update(order);
            }
            else
                itemFound = true;
        }

        return nextLocation;
    }

    private async Task<Queue<Location>> GetLocationsQueue()
    {
        var dictionary = await QueryPickingLocations();
        var locationsQueue = OrderLocationsQueue(dictionary);

        return locationsQueue;
        // return OrderPickingLocationsQueue(await QueryPickingLocations());
    }

    private async Task<SortedDictionary<int, List<Location>>> QueryPickingLocations()
    {
        var locations = await _context.Locations.Where(location => location.Floor == 1)
            .OrderBy(location => location.Isle).ToListAsync();

        var sortedLocations = CreateIsleSortedLocationsDictionary(locations);

        return sortedLocations;
    }

    private async Task<Queue<Location>> GetReplenishLocationsQueue()
    {
        var dictionary = await QueryReplenishLocations();
        var locationsQueue = OrderLocationsQueue(dictionary);

        return locationsQueue;
    }

    // These are all the locations from where the reach truckers have to take Items to replenish the Picking locations on the first floor.
    private async Task<SortedDictionary<int, List<Location>>> QueryReplenishLocations()
    {
        var locations = await _context.Locations.Where(location => location.Floor != 1)
            .OrderBy(location => location.Isle).ToListAsync();

        var sortedLocations = CreateIsleSortedLocationsDictionary(locations);

        return sortedLocations;
    }

    private Queue<Location> OrderLocationsQueue(SortedDictionary<int, List<Location>> locationsDictionary)
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