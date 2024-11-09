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

    public PaletteService(OrderPickingContext context, IUserContextService userContextService,
        IContainerService containerService)
    {
        _context = context;
        _paletteIdPattern = new(@"^pal\d{13}$");
        _userContextService = userContextService;
        _containerService = containerService;
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

    public async Task<Palette> SetContainer(string containerId)
    {
        var order = await _userContextService.QueryOngoingOrder();
        var ongoingPalette = order.OngoingPalette;
        if (ongoingPalette == null)
            throw new ArgumentException("You must first select a Palette.");

        var optionalContainer =
            await _containerService.GetOptionalContainerInProgress(containerId, ongoingPalette.Id);

        if (optionalContainer != null)
            return await SetContainerToPalette(ongoingPalette, optionalContainer);

        var container = await _containerService.CreateContainer(containerId);

        return await SetContainerToPalette(ongoingPalette, container);
    }

    private async Task<Palette> SetContainerToPalette(Palette palette, Container container)
    {
        container.PaletteId = palette.Id;
        palette.SetContainer(container);

        _context.Palettes.Update(palette);
        _context.Containers.Update(container);
        await _context.SaveChangesAsync();
        return palette;
    }
}