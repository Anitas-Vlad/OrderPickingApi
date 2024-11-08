using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class ContainerService : IContainerService
{
    private readonly OrderPickingContext _context;
    private static Regex _containerIdPattern;
    private readonly IUserContextService _userContextService;

    public ContainerService(OrderPickingContext context, IUserContextService userContextService)
    {
        _context = context;
        _containerIdPattern = new(@"^cont\d{12}$");
        _userContextService = userContextService;
    }

    public async Task<Container> QueryContainerById(string containerId)
    {
        var container = await _context.Containers.SingleOrDefaultAsync(container => container.Id == containerId);
        if (container == null)
            throw new ArgumentException("Container not found.");
        return container;
    }

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

    private void IsContainerIdValid(string containerId)
    {
        if (!_containerIdPattern.IsMatch(containerId))
            throw new ArgumentException("Invalid Container Id.");
    }
    
    public async Task<Container?> GetOptionalContainerInProgress(string containerId, string paletteId)
    {
        var container = await QueryContainerById(containerId);

        if (container != null && container.PaletteId != paletteId)
            throw new ArgumentException("Invalid Container.");

        return container;
    }

    // public async Task AddItemToContainer(int containerId, Item item) //TODO Integrate all Item updates. Location/Stock
    // {
    //     var container = await QueryContainerById(containerId);
    // }
}