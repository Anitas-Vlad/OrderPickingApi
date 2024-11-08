﻿using System.ComponentModel.DataAnnotations;

namespace OrderPickingSystem.Models;

public class Container
{
    [Required] public string Id { get; set; }
    public string PaletteId { get; set; }
    public List<Pick> Picks { get; set; } //TODO This should maybe be List<Pick> Picks

    public void AddPick(Pick newPick)//TODO This is no physical container. it only contains the picks information.
    {
        var optionalExistingPick = Picks.FirstOrDefault(pick => pick.Id == newPick.Id);
        if (optionalExistingPick == null)
        {
            Picks.Add(newPick);
        }
        else
        {
            optionalExistingPick.Quantity += newPick.Quantity;
        }
    }

    public bool CheckIfAlreadyContainsItem(int itemId)
        => Picks.Any(pick => pick.ItemId == itemId);
}