namespace OrderPickingSystem.Models;

public class Pick
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public int UserId { get; set; }
    public int ContainerId { get; set; }
}