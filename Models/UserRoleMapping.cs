using OrderPickingSystem.Models.Enums;

namespace OrderPickingSystem.Models;

public class UserRoleMapping
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public UserRole Role { get; set; }
}