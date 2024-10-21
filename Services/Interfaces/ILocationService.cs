using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface ILocationService
{
    Task<Location> QueryLocationByID(int locationId);
    Task<LocationItem> QueryLocationItemByID(int locationItemId);
    Task<Pick> CreatePick(CreatePickRequest request);
}