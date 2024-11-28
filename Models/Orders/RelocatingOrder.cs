using OrderPickingSystem.Models.Enums;

namespace OrderPickingSystem.Models.Orders;

public class RelocatingOrder : Order
{
    public RelocatingOrder()
    {
        RequiredRole = UserRole.Relocator;
    }
}