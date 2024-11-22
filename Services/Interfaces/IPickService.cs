using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Requests;

namespace OrderPickingSystem.Services.Interfaces;

public interface IPickService
{
    Task<List<Pick>> QueryPicksByContainerId(string containerId);
    Task<List<Pick>> QueryPicksByOrderId(int orderId);

    Task CreatePick(string paletteId);
    Task<Pick> CompletePick();
}