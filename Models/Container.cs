namespace OrderPickingSystem.Models;

public class Container
{
    public int Id { get; set; }
    public int PaletteId { get; set; }
    public List<Item> Items { get; set; }
}