﻿using System.ComponentModel.DataAnnotations;

namespace OrderPickingSystem.Models;

public class Item
{
    public int Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public int Quantity { get; set; }
    public int LocationId { get; set; }

    public void SubtractItem(int quantity) => Quantity -= quantity;
    public void RestockItem(int quantity) => Quantity += quantity;

    public bool HasEnoughQuantityToPick(int requestedQuantity)
        => Quantity >= requestedQuantity;

    public void RemoveFromLocation() => LocationId = 0;
    public void RelocateToLocation(Location location) 
        => LocationId = location.Id;
}