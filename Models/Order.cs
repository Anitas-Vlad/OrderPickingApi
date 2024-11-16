using System.ComponentModel.DataAnnotations;
using OrderPickingSystem.Models.Enums;
using OrderPickingSystem.Models.Requests;

namespace OrderPickingSystem.Models;

public class Order
{
    public int Id { get; set; }
    [Required] public int? CurrentUserId { get; set; }
    [Required] public OrderStatus OrderStatus { get; set; } = OrderStatus.Received; //TODO String.ValueOf() for DB
    // public List<Palette> Palettes { get; set; }
    [Required] public string Destination { get; set; }

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

     public void ThrowIfInProgress()
    {
        if (OrderStatus == OrderStatus.InProcess)
            throw new ArgumentException("This order is taken by another worker.");
    }
}