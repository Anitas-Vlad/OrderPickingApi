namespace OrderPickingSystem.Models.Requests;

public class PickRequest
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}