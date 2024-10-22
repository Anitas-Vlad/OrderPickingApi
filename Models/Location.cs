using System.ComponentModel.DataAnnotations;

namespace OrderPickingSystem.Models;

public class Location
{
    // 17-23-A
    public int Id { get; set; }
    [Required] public int Isle { get; set; }
    [Required] public int Floor { get; set; }
    [Required] public int Number { get; set; }
    [Required] public char Letter { get; set; } //TODO MUST ONLY ACCEPT ---[A B C or D]---
    public Item Item { get; set; }
}