using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Requests;

namespace OrderPickingSystem.Services.Interfaces;

public interface IPickService
{
    Task<List<Pick>> QueryPicksByContainerId(string containerId);
    Task<List<Pick>> QueryPicksByOrderId(int orderId);
    Task<List<Pick>> QueryPicksForUser(int userId, DateTime? dateTime);
    
    Task CreatePick(string paletteId);
    Task<Pick> CompletePick();
}