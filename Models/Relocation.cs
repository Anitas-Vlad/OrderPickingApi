using System.ComponentModel.DataAnnotations;
using OrderPickingSystem.Models.Enums;

namespace OrderPickingSystem.Models;

public class Relocation
{
    public int Id { get; set; }
    public RelocationType RelocationType { get; set; }
    [Required] public int UserId { get; set; }
    [Required] public int InitialLocationId { get; set; }
    public int DestinationLocationId { get; set; }
    public int ItemId { get; set; }
    public DateTime DateTime { get; set; }
}