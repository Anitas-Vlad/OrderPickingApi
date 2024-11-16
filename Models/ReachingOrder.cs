using OrderPickingSystem.Models.Requests;

namespace OrderPickingSystem.Models;

public class ReachingOrder : Order
{
    public int Id { get; set; }
    public List<RelocateItemRequest> Requests { get; set; } = new();
}