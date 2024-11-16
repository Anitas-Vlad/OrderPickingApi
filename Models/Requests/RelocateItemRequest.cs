namespace OrderPickingSystem.Models.Requests;

public class RelocateItemRequest
{
    public int ItemId { get; set; }
    public int InitialLocationId { get; set; }
    public int DestinationLocationId { get; set; }
}