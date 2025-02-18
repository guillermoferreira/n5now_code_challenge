using UserPermissionsAdmin.Models;

namespace UserPermissionsAdmin.Data
{
    public interface IElasticSearchService
    {
        Task IndexPermissionAsync(Permission permission);
        Task UpdatePermissionAsync(Permission permission);
        Task<Permission> GetPermissionByIdAsync(int permissionId);
    }
}
