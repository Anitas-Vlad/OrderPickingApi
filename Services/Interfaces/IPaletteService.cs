using OrderPickingSystem.Models;

namespace OrderPickingSystem.Services.Interfaces;

public interface IPaletteService
{
    Task<Palette> CreatePalette(int orderId);
}