using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Orders;
using OrderPickingSystem.Models.TaskRequests;

namespace OrderPickingSystem.Services.Interfaces;

public interface IRelocatingItemsService
{
    Task<Relocation> TakeItemFromLocation(int  initialLocationId);
    Task<Relocation> CompleteRelocation(int relocationId, int destinationLocationId);
    Task<RelocateItemRequest> QueryNextRelocationRequest();
}