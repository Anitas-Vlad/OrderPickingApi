using System.ComponentModel.DataAnnotations;
using OrderPickingSystem.Models.Enums;

namespace OrderPickingSystem.Models;

public class User
{
    public int Id { get; set; }
    [Required] public string Username { get; set; } // TODO Replace Email with Username in JWT
    [Required] public string PasswordHash { get; set; }
    [Required] public UserRights UserRights { get; set; } = UserRights.Worker;
    public Order? CurrentOrder { get; set; }

    public void LeaveCurrentOrder()
        => CurrentOrder = null;

    public void StartOrder(Order order)
        => CurrentOrder = order;

    public void AddAdminRights()
        => UserRights = UserRights.Admin;

    public void RemoveAdminRights()
        => UserRights = UserRights.Worker;
}