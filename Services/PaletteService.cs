using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class PaletteService : IPaletteService
{
    private readonly OrderPickingContext _context;
    private static Regex _paletteIdPattern;

    public PaletteService(OrderPickingContext context)
    {
        _context = context;
        _paletteIdPattern = new(@"^cont\d{12}$");
    }

    public async Task<Palette> CreatePalette(string PaletteId)
    {
        var palette = new Palette
        {
            Id = PaletteId,
            Containers = new List<Container>()
        };

        _context.Palettes.Add(palette);
        await _context.SaveChangesAsync();
        return palette;
    }
    
    private void IsPaletteIdValid(string paletteId)
    {
        if (!_paletteIdPattern.IsMatch(paletteId))
            throw new ArgumentException("Invalid Palette Id.");
    }
}