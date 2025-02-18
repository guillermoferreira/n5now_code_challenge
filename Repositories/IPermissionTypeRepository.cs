using UserPermissionsAdmin.Models;

namespace UserPermissionsAdmin.Repositories
{
    public interface IPermissionTypeRepository
    {
        Task<IEnumerable<PermissionType>> GetAllPermissionTypesAsync();
        Task<PermissionType> GetPermissionTypeByIdAsync(int id);
    }
}
