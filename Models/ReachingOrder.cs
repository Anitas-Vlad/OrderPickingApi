using OrderPickingSystem.Models.Requests;

namespace OrderPickingSystem.Models;

public class ReachingOrder : Order
{
    public int Id { get; set; }
    public RelocateItemRequest Request { get; set; }
}