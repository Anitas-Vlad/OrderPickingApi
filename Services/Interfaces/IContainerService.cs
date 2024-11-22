using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface IContainerService
{
    Task<Container?> QueryContainerById(string containerId);
    Task<Container> CreateContainer(string containerId);
    Task<Container?> GetOptionalContainerInProgress(string containerId, string paletteId);
    Task<Container> SetContainer(string containerId);
    void ScanContainer(string? expectedContainerId, string containerId);
    Task AddItemToContainer(Pick pick);
}