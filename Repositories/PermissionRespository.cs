using Microsoft.EntityFrameworkCore;
using UserPermissionsAdmin.Data;
using UserPermissionsAdmin.Models;

namespace UserPermissionsAdmin.Repositories
{
    public class PermissionRespository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionRespository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            return await _context.Permissions.Include(p => p.PermissionType).ToListAsync();
        }

        public async Task<Permission?> GetPermissionByIdAsync(int id)
        {
            return await _context.Permissions.Include(p => p.PermissionType)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddPermissionAsync(Permission permission)
        {
            await _context.Permissions.AddAsync(permission);
        }

        public async Task UpdatePermissionAsync(Permission permission)
        {
            _context.Permissions.Update(permission);
        }
    }
}
