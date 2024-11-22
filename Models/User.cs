using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using OrderPickingSystem.Models.Enums;

namespace OrderPickingSystem.Models;

public class User
{
    public int Id { get; set; }
    [Required] public string Username { get; set; } // TODO Replace Email with Username in JWT
    [Required] public string PasswordHash { get; set; }
    [Required] public List<UserRole> Roles { get; set; }
    public Order? CurrentOrder { get; set; }

    public void ThrowIfHasOngoingOrder()
    {
        if (CurrentOrder != null)
            throw new ArgumentException("You currently have an ongoing order.");
    }

    public void LeaveCurrentOrder()
        => CurrentOrder = null;

    public void StartOrder(Order order)
        => CurrentOrder = order;

    public bool HasRole(UserRole requiredRole)
        => Roles.Any(role => role == requiredRole);
    
    public void AddRole(UserRole role)
        => Roles.Add(role); //TODO Confirm that the user doesn't already have this role (in the implementation).
}