using System.Collections;
using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Requests;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class LocationService : ILocationService
{
    private readonly OrderPickingContext _context;
    private readonly IUserContextService _userContextService;
    private readonly IItemService _itemService;
    private readonly IPickService _pickService;

    public LocationService(OrderPickingContext context, IItemService itemService,
        IUserContextService userContextService, IPickService pickService)
    {
        _context = context;
        _itemService = itemService;
        _userContextService = userContextService;
        _pickService = pickService;
    }

    public async Task<Location> QueryLocationById(int locationId)
    {
        var location = await _context.Locations.SingleOrDefaultAsync(location => location.Id == locationId);
        if (location == null)
            throw new ArgumentException("Location not found.");
        return location;
    }

    private async Task<Location> QueryReachLocationByItemId(int itemId)
    {
        var location = await _context.Locations
            .Where(location => location.Floor > 1)
            .FirstOrDefaultAsync(location => location.Item.Id == itemId);

        if (location == null)
            throw new ArgumentException("There are no more ");

        return location;
    }

    public async void RelocateItem(RelocateItemRequest request)
    {
        var location = await QueryReachLocationByItemId(request.ItemId);
        var initialLocation = QueryLocationById(request.InitialLocationId);
        var destinationLocation = QueryLocationById(request.DestinationLocationId);
        var item = await _itemService.QueryItemById(request.ItemId);
    }

    public async Task SetPickingLocations()
    {
        var order = await _userContextService.QueryOngoingOrder();

        if (order is not PickingOrder pickingOrder)
            throw new ArgumentException("This is not a picking order.");

        var pickingLocationsQueue = await GetPickingLocationsQueue();

        var orderLocations = pickingOrder.Locations;

        while (pickingLocationsQueue.Count > 0)
        {
            var nextLocation = pickingLocationsQueue.Dequeue();

            var nextLocationItem = nextLocation.Item;
            var pickRequest = pickingOrder.GetItemById(nextLocationItem.LocationId);

            if (pickRequest == null)
                continue;

            if (!nextLocationItem.HasEnoughQuantityToPick(pickRequest.Quantity))
            {
                pickingOrder.AddReplenishItem(pickRequest);
                _context.Orders.Update(pickingOrder);
            }

            orderLocations.Enqueue(nextLocation);
        }

        _context.Orders.Update(pickingOrder);
        await _context.SaveChangesAsync();
    }

    public async Task PickFromLocation(Pick pick)
    {
        var location = await QueryLocationById(pick.LocationId);
        if (location == null)
            throw new ArgumentException("Invalid location.");

        var item = location.Item;
        
        item.SubtractItem(pick.Quantity);

        await _pickService.CompletePick();
        
        _context.Items.Update(item);
        _context.Locations.Update(location);
    }

    public async Task SetReachingLocations()
    {
        var order = await _userContextService.QueryOngoingOrder();
    }

    public async Task SetReachLocations() //TODO
    {
        var order = await _userContextService.QueryOngoingOrder();

        if (order is not ReachingOrder reachOrder)
            throw new ArgumentException("This is not a reaching order.");
    }

    public Task<Pick> PickFromLocation(CreatePickRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Location> QueryNextLocation()
    {
        var order = await _userContextService.QueryOngoingOrder();

        return order switch
        {
            PickingOrder pickingOrder => await HandleNextPickingLocation(pickingOrder),
            ReachingOrder reachingOrder => await HandleNextReachingLocation(reachingOrder),
            _ => throw new ArgumentException("Unknown type of order.")
        };
    }

    private async Task<Location> HandleNextReachingLocation(ReachingOrder reachingOrder)
    {
        throw new NotImplementedException();
    }

    private async Task<Location> HandleNextPickingLocation(PickingOrder order)
    {
        var pickingLocationsQueue = order.Locations;

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

            if (!nextLocationItem.HasEnoughQuantityToPick(pickRequest.Quantity))
                order.AddReplenishItem(pickRequest);
            else
            {
                itemFound = true;
                order.SetOngoingPickLocationItemAndQuantity(nextLocation.Id, pickRequest.ItemId, pickRequest.Quantity);
            }
        }

        _context.Orders.Update(order);
        await _context.SaveChangesAsync();

        return nextLocation;
    }

    private async Task<Queue<Location>> GetPickingLocationsQueue()
    {
        var dictionary = await QueryPickingLocations();
        var locationsQueue = SortLocationsQueue(dictionary);

        return locationsQueue;
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
        var locationsQueue = SortLocationsQueue(dictionary);

        return locationsQueue;
    }

    private async Task<SortedDictionary<int, List<Location>>> QueryReplenishLocations()
    {
        var locations = await _context.Locations.Where(location => location.Floor != 1)
            .OrderBy(location => location.Isle).ToListAsync();

        var sortedLocations = CreateIsleSortedLocationsDictionary(locations);

        return sortedLocations;
    }

    private Queue<Location> SortLocationsQueue(SortedDictionary<int, List<Location>> locationsDictionary)
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

    public void ScanLocation(int? expectedLocationId, int locationId)
    {
        if (expectedLocationId == null || expectedLocationId != locationId)
            throw new ArgumentException("Incorrect location scanned. Please verify and try again.");
    }
}