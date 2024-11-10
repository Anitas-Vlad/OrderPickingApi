using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface ILocationService
{
    Task<Location> QueryLocationById(int locationId);
    Task<Location> QueryNextLocation();
    Location ScanLocation(Location expectedLocation, int locationId);
}