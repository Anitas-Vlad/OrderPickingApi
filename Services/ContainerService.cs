using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Orders;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class ContainerService : IContainerService
{
    private readonly OrderPickingContext _context;
    private static Regex _containerIdPattern;
    private readonly IUserContextService _userContextService;
    private readonly ILocationService _locationService;

    public ContainerService(OrderPickingContext context, IUserContextService userContextService,
        ILocationService locationService)
    {
        _context = context;
        _containerIdPattern = new(@"^cont\d{12}$");
        _userContextService = userContextService;
        _locationService = locationService;
    }

    public async Task<Container?> QueryContainerById(string containerId)
        => await _context.Containers
            .Include(container => container.Picks)
            .SingleOrDefaultAsync(container => container.Id == containerId);

    public async Task<Container> CreateContainer(string containerId)
    {
        var container = new Container()
        {
            Id = containerId,
            Picks = new List<Pick>()
        };

        _context.Containers.Add(container);
        await _context.SaveChangesAsync();
        return container;
    }


    public async Task<Container?> GetOptionalContainerInProgress(string containerId, string paletteId)
    {
        var container = await QueryContainerById(containerId);

        if (container != null && container.PaletteId != paletteId)
            throw new ArgumentException("Invalid Container.");

        return container;
    }

    public async Task<Container> SetContainer(string containerId)
    {
        var order = await _userContextService.QueryOngoingOrder();

        if (order is not PickingOrder pickingOrder)
            throw new ArgumentException("This is not a picking order.");

        var ongoingPalette = pickingOrder.OngoingPalette;

        if (ongoingPalette == null)
            throw new ArgumentException("Please first set a palette.");

        var optionalContainer = await GetOptionalContainerInProgress(containerId, ongoingPalette.Id);

        if (optionalContainer != null)
            return await SetContainerToPalette(pickingOrder, ongoingPalette, optionalContainer);

        var container = await CreateContainer(containerId);

        return await SetContainerToPalette(pickingOrder, ongoingPalette, container);
    }

    public void ScanContainer(string? expectedContainerId, string containerId)
    {
        if (expectedContainerId == null || expectedContainerId != containerId)
            throw new ArgumentException("Incorrect container scanned. Please verify and try again.");
    }

    private async Task<Container> SetContainerToPalette(PickingOrder order, Palette palette, Container container)
    {
        container.PaletteId = palette.Id;
        palette.SetContainer(container);
        order.SetOngoingPickContainerId(container.Id);

        _context.Palettes.Update(palette);
        _context.Containers.Update(container);
        await _context.SaveChangesAsync();
        return container;
    }

    private void MatchesContainerIdPattern(string containerId)
    {
        if (!_containerIdPattern.IsMatch(containerId))
            throw new ArgumentException("Invalid Container Id.");
    }

    //TODO Integrate all Item updates. Location/Stock
    public async Task AddItemToContainer(Pick pick)
    {
        var container = await QueryContainerById(pick.ContainerId);
        
        if (container == null)
            throw new ArgumentException("Invalid Container.");
        
        container.Picks.Add(pick);
        
        _context.Containers.Update(container);
    }
}