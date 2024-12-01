using OrderPickingSystem.Context;
using OrderPickingSystem.Services.Interfaces;

namespace OrderPickingSystem.Services.OrderServices;

public class RelocatingOrderService : OrderService, IRelocatingOrderService
{
    public RelocatingOrderService(OrderPickingContext context,
        IUserContextService userContextService) : base(context, userContextService)
    {
    }
}