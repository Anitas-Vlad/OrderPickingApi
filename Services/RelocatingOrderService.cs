using OrderPickingSystem.Context;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class RelocatingOrderService : OrderService, IRelocatingOrderService
{
    public RelocatingOrderService(OrderPickingContext context, IPaletteService paletteService, 
        IUserContextService userContextService) : base(context, paletteService, userContextService)
    {
    }
}