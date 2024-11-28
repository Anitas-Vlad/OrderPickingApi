using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Orders;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class PaletteService : IPaletteService
{
    private readonly OrderPickingContext _context;
    private readonly IUserContextService _userContextService;
    private readonly IContainerService _containerService;
    private readonly IPickService _pickService;
    private static Regex _paletteIdPattern;

    public PaletteService(OrderPickingContext context, IUserContextService userContextService,
        IContainerService containerService, IPickService pickService)
    {
        _context = context;
        _paletteIdPattern = new(@"^pal\d{13}$");
        _userContextService = userContextService;
        _containerService = containerService;
        _pickService = pickService;
    }

    public async Task<Palette?> QueryPaletteById(string paletteId)
        => await _context.Palettes.FirstOrDefaultAsync(palette => palette.Id == paletteId);

    public async Task<Palette> CreatePalette(string paletteId)
    {
        var palette = new Palette
        {
            Id = paletteId,
            Containers = new List<Container>()
        };

        _context.Palettes.Add(palette);
        await _context.SaveChangesAsync();
        return palette;
    }

    public async Task<Palette> SetPalette(string paletteId)
    {
        var order = await _userContextService.QueryOngoingOrder();

        if (order is not PickingOrder pickingOrder)
            throw new ArgumentException("This is not a picking order.");

        var optionalPalette = await GetOptionalPaletteInProgress(paletteId, pickingOrder.Id);

        if (optionalPalette == null)
        {
            var palette = await CreatePalette(paletteId);

            await SetPaletteToOrder(palette, pickingOrder);
            return palette;
        }

        await SetPaletteToOrder(optionalPalette, pickingOrder);
        return optionalPalette;
    }

    private async Task SetPaletteToOrder(Palette palette, PickingOrder order)
    {
        palette.OrderId = order.Id;
        order.SetOngoingPalette(palette);
        await _pickService.CreatePick(palette.Id);

        _context.Orders.Update(order);
        _context.Palettes.Update(palette);
        await _context.SaveChangesAsync();
    }

    private void MatchesPaletteIdPattern(string paletteId)
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
}