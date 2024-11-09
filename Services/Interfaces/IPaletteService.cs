using OrderPickingSystem.Controllers;
using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface IPaletteService
{
    Task<Palette?> QueryPaletteById(string paletteId);
    Task<Palette> CreatePalette(string paletteId);
    Task<Palette?> GetOptionalPaletteInProgress(string paletteId, int orderId);
    Task<Palette> SetContainer(string containerId);
}