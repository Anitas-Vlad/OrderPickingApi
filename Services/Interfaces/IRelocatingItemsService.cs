using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface IRelocatingItemsService
{
    Task<Relocation> TakeItemFromLocation(int  initialLocationId);
}