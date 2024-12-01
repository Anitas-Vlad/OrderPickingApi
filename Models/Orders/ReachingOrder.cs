using OrderPickingSystem.Models.Enums;
using OrderPickingSystem.Models.Requests;
using OrderPickingSystem.Models.TaskRequests;

namespace OrderPickingSystem.Models.Orders;

public class ReachingOrder : Order
{
    public int Id { get; set; }
    public RelocateItemRequest Request { get; set; }

    public ReachingOrder()
    {
        RequiredRole = UserRole.Reacher;
    }
}