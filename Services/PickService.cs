using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Orders;
using OrderPickingSystem.Models.Requests;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class PickService : IPickService
{
    private readonly OrderPickingContext _context;
    private readonly IUserContextService _userContextService;
    private readonly ILocationService _locationService;
    private readonly IContainerService _containerService;


    public PickService(OrderPickingContext context, IUserContextService userContextService,
        ILocationService locationService, IContainerService containerService)
    {
        _context = context;
        _userContextService = userContextService;
        _locationService = locationService;
        _containerService = containerService;
    }

    public async Task<Pick> CompletePick()
    {
        var order = await _userContextService.QueryOngoingOrder();

        if (order is not PickingOrder pickingOrder)
            throw new ArgumentException("Invalid Order.");

        var user = await _userContextService.QueryPersonalAccount();

        var pick = pickingOrder.OngoingPick;
        pick.UserId = user.Id;
        pick.DateTime = DateTime.Now;
        pick.OrderId = pickingOrder.Id;

        await _locationService.PickFromLocation(pick);
        await _containerService.AddItemToContainer(pick);

        _context.Picks.Update(pick);
        await _context.SaveChangesAsync();
        return pick;
    }

    public async Task<List<Pick>> QueryAllPicks()
        => await _context.Picks.ToListAsync();

    public async Task<List<Pick>> QueryPicksByContainerId(string containerId)
    {
        var container = await _containerService.QueryContainerById(containerId);

        if (container == null)
            throw new ArgumentException("Container not found.");

        return container.Picks;
    }

    public async Task<List<Pick>> QueryPicksByOrderId(int orderId) =>
        await _context.Picks
            .Where(pick => pick.OrderId == orderId)
            .OrderBy(pick => pick.DateTime)
            .ToListAsync();

    public async Task<List<Pick>> QueryPicksForUser(int orderId, DateTime? dateTime)
    {
        var picks = _context.Picks
            .Where(pick => pick.OrderId == orderId)
            .OrderBy(pick => pick.DateTime);

        if (!dateTime.HasValue)
            return await picks.ToListAsync();

        var result = await picks
            .Where(pick => pick.DateTime.Day == dateTime.Value.Day)
            .ToListAsync();
        return result;
    }

    public async Task<List<Pick>> QueryPicksForUser(int userId)
        => await _context.Picks
            .Where(pick => pick.UserId == userId)
            .OrderBy(pick => pick.DateTime)
            .ToListAsync();

    public async Task<List<Pick>> QueryPicksForUser(int userId, DateTime dateTime)
        => await _context.Picks
            .Where(pick => pick.UserId == userId && pick.DateTime.Day == dateTime.Day)
            .OrderBy(pick => pick.DateTime)
            .ToListAsync();

    public async Task CreatePick(string paletteId)
    {
        var pick = new Pick { PaletteId = paletteId };

        _context.Picks.Add(pick);
        await _context.SaveChangesAsync();
    }
}