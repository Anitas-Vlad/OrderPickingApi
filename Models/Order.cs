using System.ComponentModel.DataAnnotations;
using OrderPickingSystem.Models.Enums;

namespace OrderPickingSystem.Models;

public class Order
{
    public int Id { get; set; }
    [Required] public int? CurrentUserId { get; set; }
    [Required] public OrderStatus OrderStatus { get; set; } = OrderStatus.Received; //TODO String.ValueOf() for DB
    [Required] public List<Palette> Palettes { get; set; }
    [Required] public string Destination { get; set; }
    [Required] public Queue<Item> Items { get; set; } //TODO Queue<PickRequest>
    [Required] public Queue<Item> ReplenishItems { get; set; } //TODO Queue<PickRequest>

    [Required(ErrorMessage = "Mobile number is required.")]
    [Phone(ErrorMessage = "Please enter a valid number.")]
    public string ContactNumber { get; set; }

    public void EnqueueReplenishItem(Item item)
        => ReplenishItems.Enqueue(item);

    public Item? GetItemById(int itemId) 
        => Items.FirstOrDefault(item => item.Id == itemId);
}