using UserPermissionsAdmin.Commands;
using UserPermissionsAdmin.Models;
using UserPermissionsAdmin.Queries;

namespace UserPermissionsAdmin.Services
{
    public interface IPermissionService
    {
        Task<Permission> GetPermissionByIdAsync(GetPermissionQuery query);
        Task<IEnumerable<Permission>> GetAllPermissionsAsync();
        Task<Permission> CreatePermissionAsync(CreatePermissionCommand command);
        Task<Permission> UpdatePermissionAsync(UpdatePermissionCommand command);
    }
}
