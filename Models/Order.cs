namespace OrderPickingSystem.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public Palette Palette { get; set; }
    public Location Location { get; set; }
}