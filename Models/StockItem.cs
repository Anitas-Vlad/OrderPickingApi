using System.ComponentModel.DataAnnotations;

namespace OrderPickingSystem.Models;

public class StockItem
{
    public int Id { get; set; }
    [Required] public int Name { get; set; }
    [Required] public int Quantity { get; set; }
}