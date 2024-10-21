using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface ILocationService
{
    Task<Location> QueryLocationById(int locationId);
    Task<Pick> CreatePick(CreatePickRequest request);
    Task PickFromLocation(CreatePickRequest request);
}