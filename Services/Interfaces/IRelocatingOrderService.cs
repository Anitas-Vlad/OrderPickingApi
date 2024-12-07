using OrderPickingSystem.Models.TaskRequests;

namespace OrderPickingSystem.Services.Interfaces;

public interface IRelocatingOrderService
{
    Task TakeItemFromLocation(int  initialLocationId);
}