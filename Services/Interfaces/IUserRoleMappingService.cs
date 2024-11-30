using OrderPickingSystem.Models;
using OrderPickingSystem.Models.Enums;

namespace OrderPickingSystem.Services.Interfaces;

public interface IUserRoleMappingService
{
    UserRoleMapping CreateUserRoleMapping(int userId, UserRole role);
    void RemoveUserRoleMapping(UserRoleMapping userRoleMapping);
}