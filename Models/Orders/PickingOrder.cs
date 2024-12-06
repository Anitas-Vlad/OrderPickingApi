using System.ComponentModel.DataAnnotations;
using OrderPickingSystem.Models.Enums;
using OrderPickingSystem.Models.TaskRequests;

namespace OrderPickingSystem.Models.Orders;

public class PickingOrder : Order
{
    public PickingOrder()
    {
        RequiredRole = UserRole.Picker;
    }

    public List<Palette> Palettes { get; set; } = new();
    public Palette? OngoingPalette { get; set; }
    public List<Pick> Picks { get; set; } = new();
    public Pick OngoingPick { get; set; } = new();
    [Required] public List<PickRequest> RequestedItems { get; set; } = new();
    [Required] public List<PickRequest> ReplenishedRequestedItems { get; set; } = new();
    public Queue<Location> Locations { get; set; } = new();

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

    public void ThrowIfCannotBeTaken() //TODO delete or refactor
    {
        ThrowIfInProgress();
        if (!RequestedItems.Any() || !ReplenishedRequestedItems.Any())
            throw new ArgumentException("There are no more items to be picked.");
    }

    public void UpdateAfterReplenishment(int itemId)
    {
        var request = ReplenishedRequestedItems.First(requestedItem => requestedItem.ItemId == itemId);
        ReplenishedRequestedItems.Remove(request);
        RequestedItems.Add(request);
    }
}