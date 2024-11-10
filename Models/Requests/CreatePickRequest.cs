namespace OrderPickingSystem.Models.Requests;

public class CreatePickRequest
{
    public int ItemId { get; set; }
    public int UserId { get; set; }
    public string ContainerId { get; set; }
    public int LocationId { get; set; }
    public int Quantity { get; set; }
}