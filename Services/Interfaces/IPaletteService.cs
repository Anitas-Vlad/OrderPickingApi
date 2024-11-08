using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface IPaletteService
{
    Task<Palette> CreatePalette(string paletteId);
    Task<Palette?> GetOptionalPaletteInProgress(string paletteId, int orderId);
}