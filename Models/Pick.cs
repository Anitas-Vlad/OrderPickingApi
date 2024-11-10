using System.ComponentModel.DataAnnotations;

namespace OrderPickingSystem.Models;

public class Pick
{
    public int Id { get; set; }
    [Required] public int LocationId { get; set; }
    [Required] public int ItemId { get; set; }
    [Required] public int UserId { get; set; }
    [Required] public string ContainerId { get; set; }
    [Required] public int Quantity { get; set; }
    [Required] public DateTime DateTime { get; set; }
}