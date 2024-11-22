using OrderPickingSystem.Context;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services;

public class RelocateOrderService : OrderService
{
    public RelocateOrderService(OrderPickingContext context, IPaletteService paletteService, 
        IUserContextService userContextService) : base(context, paletteService, userContextService)
    {
    }
}