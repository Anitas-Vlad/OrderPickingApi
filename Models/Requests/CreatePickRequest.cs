namespace OrderPickingSystem.Models;

public class CreatePickRequest
{
    public int LocationId { get; set; }
    public int ItemId { get; set; }
    public int ContainerId { get; set; }
    public int UserId { get; set; }
    public int Quantity { get; set; }
}