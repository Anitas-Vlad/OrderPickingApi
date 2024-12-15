using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface ILocationService
{
    Task<Location> QueryLocationById(int locationId);
    Task<Location> QueryNextPickingLocation();
    void ScanLocation(int? expectedLocation, int locationId);
    Task SetPickingLocations();
    Task SetReachingLocations();
    Task PickFromLocation(Pick pick);
}