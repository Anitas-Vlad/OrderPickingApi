using System.ComponentModel.DataAnnotations;

namespace OrderPickingSystem.Models;

public class Relocation
{
    public int Id { get; set; }
    [Required] public int UserId { get; set; }
    [Required] public int InitialLocationId { get; set; }
    [Required] public int DestinationLocationId { get; set; }
    public DateTime DateTime { get; set; }
}