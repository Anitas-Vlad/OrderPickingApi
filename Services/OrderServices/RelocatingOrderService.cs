using OrderPickingSystem.Context;
using OrderPickingSystem.Models;
using OrderPickingSystem.Models.TaskRequests;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services.OrderServices;

public class RelocatingOrderService : OrderService, IRelocatingOrderService
{
    private readonly ILocationService _locationService;
    private readonly OrderPickingContext _context;
    private readonly IUserContextService _userContextService;

    public RelocatingOrderService(OrderPickingContext context,
        IUserContextService userContextService, ILocationService locationService) : base(context, userContextService)
    {
        _context = context;
        _locationService = locationService;
        _userContextService = userContextService;
    }

    private Relocation CreateRelocation(int initialLocationId, int userId)
    {
        var relocation = new Relocation()
        {
            InitialLocationId = initialLocationId,
            UserId = userId
        };
        
        _context.Relocations.Add(relocation);

        return relocation;
    }

    public async Task TakeItemFromLocation(int initialLocationId)
    {
        if (initialLocationId == 0)
            throw new ArgumentException("Initial location id missing.");
        
        var userId = _userContextService.GetUserId();
        var location = await _locationService.QueryLocationById(initialLocationId);
        var item = location.Item;

        var relocation = CreateRelocation(initialLocationId, userId);

        location.RemoveItem();
        item!.RemoveFromLocation();

        _context.Relocations.Update(relocation);
        _context.Locations.Update(location);
        _context.Items.Update(item);

        await _context.SaveChangesAsync();
    }
}