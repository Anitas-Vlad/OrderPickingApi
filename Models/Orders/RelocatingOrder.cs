using OrderPickingSystem.Models.Enums;
using OrderPickingSystem.Models.TaskRequests;

namespace OrderPickingSystem.Models.Orders;

public class RelocatingOrder : Order
{
    public Queue<RelocateItemRequest> Requests { get; set; }
    public RelocateItemRequest? OngoingRequest { get; set; }

    public RelocatingOrder()
    {
        RequiredRole = UserRole.Relocator;
    }
}