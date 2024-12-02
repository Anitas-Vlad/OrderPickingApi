using OrderPickingSystem.Models.Enums;

namespace OrderPickingSystem.Models;

public class UserRoleMapping
{
    public int UserId { get; set; }

    public UserRole Role { get; set; }
}