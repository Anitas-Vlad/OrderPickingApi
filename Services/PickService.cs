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
    private readonly IItemService _itemService;
    private readonly IUserContextService _userContextService;
    private readonly ILocationService _locationService;
    private readonly IContainerService _containerService;
    private readonly IOrderService _orderService;
    

    public PickService(OrderPickingContext context, IItemService itemService, IUserContextService userContextService,
        ILocationService locationService, IContainerService containerService, IOrderService orderService)
    {
        _context = context;
        _itemService = itemService;
        _userContextService = userContextService;
        _locationService = locationService;
        _containerService = containerService;
        _orderService = orderService;
    }

    public async Task<Pick> CompletePick()
    {
        var order = await _userContextService.QueryOngoingOrder();

        if (order == null) throw new ArgumentException("No ongoing order found.");
        if (order is not PickingOrder pickingOrder) throw new ArgumentException("Invalid Order.");

        var user = await _userContextService.QueryPersonalAccount();

        var pick = pickingOrder.OngoingPick;
        pick.UserId = user.Id;
        pick.DateTime = DateTime.Now;

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

    public async Task<List<Pick>> QueryPicksByOrderId(int orderId)
    {
        throw new NotImplementedException();
    }

    public async Task CreatePick(string paletteId)
    {
        var pick = new Pick { PaletteId = paletteId };

        _context.Picks.Add(pick);
        await _context.SaveChangesAsync();
    }
}