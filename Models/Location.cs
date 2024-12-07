using System.ComponentModel.DataAnnotations;

namespace OrderPickingSystem.Models;

public class Location
{
    // 17-23-A-1
    public int Id { get; set; }
    [Required] public int Isle { get; set; }
    [Required] public int Number { get; set; }
    [Required] public char Letter { get; set; } //TODO MUST ONLY ACCEPT ---[A B C or D]---
    [Required] public int Floor { get; set; }
    public Item? Item { get; set; }

    // public bool HasEnoughItemQuantity(int requestedQuantity)
    //     => Item.Quantity >= requestedQuantity;
    
    public void RemoveItem() => Item = null;

    public int GetItemId()
    {
        if (Item == null)
            throw new ArgumentException("There is no item in this location.");
        
        return Item.Id;
    }
}