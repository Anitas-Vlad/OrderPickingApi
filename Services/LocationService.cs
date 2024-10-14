using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services;

public class LocationService
{
    private readonly OrderPickingContext _context;

    public LocationService(OrderPickingContext context)
    {
        _context = context;
    }

    public async Task<Location> QueryLocationByID(int locationId)
    {
        var location = await _context.Locations.SingleOrDefaultAsync(location => location.Id == locationId);
        if (location == null)
            throw new ArgumentException("Location not found.");
        return location;
    }

    public async Task PickFromLocation(int locationId, Item item)
    {
        var location = await QueryLocationByID(locationId);
        
        
    }
}