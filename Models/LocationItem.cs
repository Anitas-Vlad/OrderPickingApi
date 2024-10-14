using System.ComponentModel.DataAnnotations;

namespace OrderPickingSystem.Models;

public class LocationItem
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    [Required] public int Name { get; set; }
    [Required] public int Quantity { get; set; }
}