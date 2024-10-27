using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface ILocationService
{
    Task<Location> QueryLocationById(int locationId);
    Task<Queue<Location>> QueryPickingLocationsQueue();
    Task<Queue<Location>> QueryReplenishLocationsQueue();
}