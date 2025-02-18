using Microsoft.EntityFrameworkCore;
using UserPermissionsAdmin.Data;
using UserPermissionsAdmin.Models;

namespace UserPermissionsAdmin.Repositories
{
    public class PermissionTypeRespository : IPermissionTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionTypeRespository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PermissionType>> GetAllPermissionTypesAsync()
        {
            return await _context.PermissionTypes.ToListAsync();
        }

        public async Task<PermissionType> GetPermissionTypeByIdAsync(int id)
        {
            return await _context.PermissionTypes.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
