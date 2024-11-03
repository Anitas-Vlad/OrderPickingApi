namespace OrderPickingSystem.Models;

public class Palette
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public List<Container> Containers { get; set; }
}