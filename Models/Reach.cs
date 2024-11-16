using System.ComponentModel.DataAnnotations;

namespace OrderPickingSystem.Models;

public class Reach
{
    public int Id { get; set; }
    [Required] public int InitialLocationId { get; set; }
    [Required] public int DestinationLocationId { get; set; }
    [Required] public int ItemId { get; set; }
    [Required] public int UserId { get; set; }
    [Required] public DateTime DateTime { get; set; }
}