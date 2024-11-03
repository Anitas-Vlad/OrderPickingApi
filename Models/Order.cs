using System.ComponentModel.DataAnnotations;
using OrderPickingSystem.Models.Enums;
using OrderPickingSystem.Models.Requests;

namespace OrderPickingSystem.Models;

public class Order
{
    public int Id { get; set; }
    [Required] public int? CurrentUserId { get; set; }
    [Required] public OrderStatus OrderStatus { get; set; } = OrderStatus.Received; //TODO String.ValueOf() for DB
    [Required] public List<Palette> Palettes { get; set; }
    [Required] public string Destination { get; set; }
    [Required] public List<Pick> Picks { get; set; } = new();
    [Required] public Queue<PickRequest> RequestedItems { get; set; } //TODO Queue<PickRequest>
    [Required] public Queue<PickRequest> ReplenishedRequestedItems { get; set; } //TODO Queue<PickRequest>

    [Required(ErrorMessage = "Mobile number is required.")]
    [Phone(ErrorMessage = "Please enter a valid number.")]
    public string ContactNumber { get; set; }

    public void SetCurrentUserId(int userId)
        => CurrentUserId = userId;

    public void EnqueueReplenishItem(PickRequest request)
        => ReplenishedRequestedItems.Enqueue(request);

    public PickRequest? GetItemById(int itemId)
        => RequestedItems.FirstOrDefault(request => request.ItemId == itemId);
}