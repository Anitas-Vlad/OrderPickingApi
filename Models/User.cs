using System.ComponentModel.DataAnnotations;
using OrderPickingSystem.Models.Enums;

namespace OrderPickingSystem.Models;

public class User
{
    public int Id { get; set; }
    [Required] public string Username { get; set; } // TODO Replace Email with Username in JWT
    [Required] public string PasswordHash { get; set; }
    [Required] public UserRole Role { get; set; } = UserRole.Worker;
    public Order? CurrentOrder { get; set; }

    public void LeaveCurrentOrder()
        => CurrentOrder = null;

    public void StartOrder(Order order)
        => CurrentOrder = order;

    public void AddAdminRights()
        => Role = UserRole.Admin;

    public void RemoveAdminRights()
        => Role = UserRole.Worker;
}