using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface ILocationService
{
    Task<Location> QueryLocationById(int locationId);
    Task<Pick> CreatePick(CreatePickRequest request);
    Task PickFromLocation(CreatePickRequest request);
    Task<Queue<Location>> QueryPickingLocationsQueue();
    Task<SortedDictionary<int, List<Location>>> QueryPickingLocations();
    SortedDictionary<int, List<Location>> createIsleSortedLocationsDictionary(List<Location> locations);
}