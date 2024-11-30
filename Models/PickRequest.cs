using System.ComponentModel.DataAnnotations;
using OrderPickingSystem.Models.Orders;

namespace OrderPickingSystem.Models;

public class PickRequest
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}