using System.ComponentModel.DataAnnotations;

namespace OrderPickingSystem.Models;

public class Palette
{
    [Required] public string Id { get; set; }
    public int OrderId { get; set; }
    public List<Container> Containers { get; set; }
}