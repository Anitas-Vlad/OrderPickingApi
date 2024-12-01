using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using OrderPickingSystem.Models.Enums;
using OrderPickingSystem.Models.Orders;

namespace OrderPickingSystem.Models;

public class User
{
    public int Id { get; set; }
    [Required] public string Username { get; set; } // TODO Replace Email with Username in JWT
    [Required] public string PasswordHash { get; set; }
    // public List<UserRole> Roles { get; set; } = new List<UserRole>();
    public List<UserRoleMapping> UserRoles { get; set; }
    public Order? CurrentOrder { get; set; }

    public void LeaveCurrentOrder()
        => CurrentOrder = null;

    public void StartOrder(Order order)
        => CurrentOrder = order;

    public List<UserRole> GetRoles() 
        => UserRoles.Select(urm => urm.Role).ToList();
    
    public bool HasRole(UserRole requiredRole) 
        => UserRoles.Any(urm => urm.Role == requiredRole);

    public void AddRole(UserRoleMapping role)
        => UserRoles.Add(role);

    public void RemoveRole(UserRoleMapping role) 
        => UserRoles.Remove(role);

    public UserRoleMapping? GetUserRoleMapping(UserRole userRole)
    {
        return UserRoles.FirstOrDefault(urm => urm.Role == userRole);
    }
}