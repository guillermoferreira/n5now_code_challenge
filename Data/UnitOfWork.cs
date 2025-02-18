
using UserPermissionsAdmin.Repositories;

namespace UserPermissionsAdmin.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IPermissionRepository Permissions { get; }
        public IPermissionTypeRepository PermissionTypes { get; }

        public UnitOfWork(ApplicationDbContext context, IPermissionRepository permissionRepository, 
                            IPermissionTypeRepository permissionTypes)
        {
            _context = context;
            Permissions = permissionRepository;
            PermissionTypes = permissionTypes;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
