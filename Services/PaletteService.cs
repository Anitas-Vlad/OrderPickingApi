using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class PaletteService : IPaletteService
{
    private readonly OrderPickingContext _context;

    public PaletteService(OrderPickingContext context)
    {
        _context = context;
    }

    public async Task<Palette> CreatePalette(int orderId)
    {
        var palette = new Palette
        {
            Containers = new List<Container>(),
            OrderId = orderId
        };

        _context.Palettes.Add(palette);
        await _context.SaveChangesAsync();
        return palette;
    }
}