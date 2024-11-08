using System.ComponentModel.DataAnnotations;
using OrderPickingSystem.Models.Enums;
using OrderPickingSystem.Models.Requests;

namespace OrderPickingSystem.Models;

public class Order
{
    public int Id { get; set; }
    [Required] public int? CurrentUserId { get; set; }
    [Required] public OrderStatus OrderStatus { get; set; } = OrderStatus.Received; //TODO String.ValueOf() for DB
    public List<Palette> Palettes { get; set; }
    public Palette? OngoingPalette { get; set; }
    public List<Pick> Picks { get; set; }
    [Required] public string Destination { get; set; }
    [Required] public Queue<PickRequest> RequestedItems { get; set; } //TODO Queue<PickRequest>
    [Required] public Queue<PickRequest> ReplenishedRequestedItems { get; set; } //TODO Queue<PickRequest>

    [Required(ErrorMessage = "Mobile number is required.")]
    [Phone(ErrorMessage = "Please enter a valid number.")]
    public string ContactNumber { get; set; }

    // public Palette? GetOngoingPalette()
    // {
    //     if (OngoingPalette == null)
    //     {
    //         throw new ArgumentException("There is no palette in progress.");
    //     }
    //
    //     return OngoingPalette;
    // }

    public void ThrowIfCannotBePicked()
    {
        if (OrderStatus == OrderStatus.Picking)
            throw new ArgumentException("This order is taken by another worker.");
        if (!RequestedItems.Any() || !ReplenishedRequestedItems.Any())
            throw new ArgumentException("There are no more items to be picked.");
    }

    public void EnqueueReplenishItem(PickRequest request)
        => ReplenishedRequestedItems.Enqueue(request);

    public PickRequest? GetItemById(int itemId)
        => RequestedItems.FirstOrDefault(request => request.ItemId == itemId);

    public void SetOngoingPalette(Palette palette)
    {
        if (!Palettes.Contains(palette))
        {
            OngoingPalette = palette;
            Palettes.Add(OngoingPalette);
        }
        else
            OngoingPalette = palette;
    }
}