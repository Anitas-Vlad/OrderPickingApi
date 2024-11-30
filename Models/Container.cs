using System.ComponentModel.DataAnnotations;

namespace OrderPickingSystem.Models;

public class Container
{
    [Required] public string Id { get; set; }
    public string PaletteId { get; set; }
    public List<Pick> Picks { get; set; } = new();

    // public void AddPick(Pick newPick)//TODO Remember this is not a physical container. it only contains the picks information.
    // {
    //     var optionalExistingPick = Picks.FirstOrDefault(pick => pick.Id == newPick.Id);
    //     if (optionalExistingPick == null)
    //     {
    //         Picks.Add(newPick);
    //     }
    //     else
    //     {
    //         optionalExistingPick.Quantity += newPick.Quantity;
    //     }
    // }

    public void AddPick(Pick pick)
        => Picks.Add(pick);

    public bool ContainsItem(int itemId)
        => Picks.Any(pick => pick.ItemId == itemId);
}