using System.ComponentModel.DataAnnotations;
using OrderPickingSystem.Models.Enums;

namespace OrderPickingSystem.Models.Orders;

public class PickingOrder : Order
{
    public PickingOrder()
    {
        RequiredRole = UserRole.Picker;
    }

    public List<Palette> Palettes { get; set; }
    public Palette? OngoingPalette { get; set; }
    public List<Pick> Picks { get; set; }
    public Pick OngoingPick { get; set; }
    [Required] public List<PickRequest> RequestedItems { get; set; }
    [Required] public List<PickRequest> ReplenishedRequestedItems { get; set; }
    public Queue<Location> Locations { get; set; } //TODO Check

    public void SetOngoingPickPaletteId(string paletteId)
        => OngoingPick.PaletteId = paletteId;

    public void SetOngoingPickContainerId(string containerId)
        => OngoingPick.ContainerId = containerId;

    public void SetOngoingPickLocationItemAndQuantity(int locationId, int itemId, int quantity)
    {
        OngoingPick.LocationId = locationId;
        OngoingPick.ItemId = itemId;
        OngoingPick.Quantity = quantity;
    }

    public void AddReplenishItem(PickRequest request)
        => ReplenishedRequestedItems.Add(request);

    public PickRequest? GetItemById(int itemId)
        => RequestedItems.FirstOrDefault(request => request.ItemId == itemId);

    public void SetOngoingPalette(Palette palette)
    {
        if (!Palettes.Contains(palette))
        {
            OngoingPalette = palette;
            Palettes.Add(OngoingPalette);
        }
        else OngoingPalette = palette;
    }

    public void ThrowIfCannotBeTaken()
    {
        ThrowIfInProgress();
        if (!RequestedItems.Any() || !ReplenishedRequestedItems.Any())
            throw new ArgumentException("There are no more items to be picked.");
    }

    public void UpdateAfterReplenishment(int itemId)
    {
        var request = ReplenishedRequestedItems.First(request => request.ItemId == itemId);
        ReplenishedRequestedItems.Remove(request);
        RequestedItems.Add(request);
    }
}