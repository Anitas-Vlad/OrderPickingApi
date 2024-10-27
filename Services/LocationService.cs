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
    
    public async Task<Queue<Location>> QueryPickingLocationsQueue()
    {
        var dictionary = await QueryPickingLocations();
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
    
    // These are all the 1st floor picking locations.
    //TODO Maybe there should be another table in the DB for the PickingLocations so that this request is not made every time an order is starte.
    public async Task<SortedDictionary<int, List<Location>>> QueryPickingLocations()
    {
        var locations = await _context.Locations.Where(location => location.Floor == 1)
            .OrderBy(location => location.Isle).ToListAsync();

        var sortedLocations = createIsleSortedLocationsDictionary(locations);

        return sortedLocations;
    }


    public SortedDictionary<int, List<Location>> createIsleSortedLocationsDictionary(List<Location> locations)
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

    public async Task<Pick> CreatePick(CreatePickRequest request) //TODO Maybe don t need this method
    {
        var pick = new Pick
        {
            LocationId = request.LocationId,
            ItemId = request.LocationId,
            UserId = request.UserId,
            ContainerId = request.ContainerId
        };

        await _context.Picks.AddAsync(pick);
        return pick;
    }

    public async Task PickFromLocation(CreatePickRequest request)
    {
        var locationItem = await _itemService.QueryItemById(request.ItemId);
        if (!locationItem.CheckIfQuantityIsEnoughToPick(request)) 
            await _pickService.CreateReplenish();

        var pick = await CreatePick(request);
    }
}