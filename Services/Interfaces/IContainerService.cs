using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface IContainerService
{
    Task<Container> CreateContainer(string containerId);
    Task<Container?> GetOptionalContainerInProgress(string containerId, string paletteId);
}