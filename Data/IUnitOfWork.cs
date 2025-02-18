using UserPermissionsAdmin.Repositories;

namespace UserPermissionsAdmin.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IPermissionRepository Permissions { get; }
        IPermissionTypeRepository PermissionTypes { get; }
        Task<int> SaveChangesAsync();
    }
}
