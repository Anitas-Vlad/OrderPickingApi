using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface ILocationService
{
    Task<Location> QueryLocationById(int locationId);
    Task<Location> QueryNextLocation();
    void ScanLocation(int? expectedLocation, int locationId);
    Task SetPickingLocations();
    Task PickFromLocation(Pick pick);
}