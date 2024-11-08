using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class PaletteService : IPaletteService
{
    private readonly OrderPickingContext _context;
    private readonly IUserContextService _userContextService;
    private readonly IContainerService _containerService;
    private static Regex _paletteIdPattern;

    public PaletteService(OrderPickingContext context, IUserContextService userContextService, IContainerService containerService)
    {
        _context = context;
        _paletteIdPattern = new(@"^pal\d{13}$");
        _userContextService = userContextService;
        _containerService = containerService;
    }

    public async Task<Palette?> QueryPaletteById(string paletteId)
        => await _context.Palettes.FirstOrDefaultAsync(palette => palette.Id == paletteId);

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

    public async Task<Palette?> GetOptionalPaletteInProgress(string paletteId, int orderId)
    {
        var palette = await QueryPaletteById(paletteId);

        if (palette != null && palette.OrderId != orderId)
            throw new ArgumentException("Invalid Palette.");
        
        return palette;
    }
    
    // public async Task<Palette> SetContainer(string paletteId)
    // {
    //     var order = await _userContextService.QueryOngoingOrder();
    //     var optionalPalette = 
    //         
    //         
    //     // var palette = await CreatePalette(paletteId); //TODO QueryOngoingPalette
    //     // await _paletteService.CheckIfPaletteExistsInOtherOrder(paletteId, order.Id);
    //     //
    //     // palette.OrderId = order.Id;
    //     // order.Palettes.Add(palette);
    //     //
    //     // _context.Orders.Update(order);
    //     // _context.Palettes.Update(palette);
    //     // await _context.SaveChangesAsync();
    //     //
    //     // return order;
    // }
}