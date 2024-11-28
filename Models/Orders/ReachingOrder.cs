using OrderPickingSystem.Models.Enums;
using OrderPickingSystem.Models.Requests;

namespace OrderPickingSystem.Models.Orders;

public class ReachingOrder : Order
{
    public ReachingOrder()
    {
        RequiredRole = UserRole.Reacher;
    }
    
    public int Id { get; set; }
    public RelocateItemRequest Request { get; set; }
}