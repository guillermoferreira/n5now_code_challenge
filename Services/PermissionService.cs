using UserPermissionsAdmin.Commands;
using UserPermissionsAdmin.Data;
using UserPermissionsAdmin.Models;
using UserPermissionsAdmin.Queries;

namespace UserPermissionsAdmin.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticSearchService _elasticSearch;

        public PermissionService(IUnitOfWork unitOfWork, IElasticSearchService elasticSearchService)
        {
            _unitOfWork = unitOfWork;
            _elasticSearch = elasticSearchService;
        }

        public async Task<Permission> CreatePermissionAsync(CreatePermissionCommand command)
        {
            var permissionType = await _unitOfWork.PermissionTypes.GetPermissionTypeByIdAsync(command.PermissionTypeId);

            if (permissionType == null)
            {
                throw new KeyNotFoundException($"Permission type with id {command.PermissionTypeId} was not found");
            }

            var permission = new Permission
            {
                EmployeeForename = command.EmployeeForename,
                EmployeeSurname = command.EmployeeSurname,
                PermissionDate = DateTime.UtcNow,
                PermissionType = permissionType
            };

            await _unitOfWork.Permissions.AddPermissionAsync(permission);
            
            permission.Id = await _unitOfWork.SaveChangesAsync();

            await _elasticSearch.IndexPermissionAsync(permission);

            return permission;
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            return await _unitOfWork.Permissions.GetAllPermissionsAsync();
        }

        public async Task<Permission> GetPermissionByIdAsync(GetPermissionQuery query)
        {
            var permission = await _unitOfWork.Permissions.GetPermissionByIdAsync(query.PermissionId);

            if (permission == null)
            {
                throw new KeyNotFoundException($"Permission with id {query.PermissionId} was not found");
            }

            return permission;
        }

        public async Task<Permission> UpdatePermissionAsync(UpdatePermissionCommand command)
        {            
            var permission = await _unitOfWork.Permissions.GetPermissionByIdAsync(command.Id);

            if (permission == null)
            {
                throw new KeyNotFoundException($"Permission with id {command.Id} was not found");
            }

            var permissionType = await _unitOfWork.PermissionTypes.GetPermissionTypeByIdAsync(command.PermissionTypeId);

            if (permissionType == null)
            { 
                throw new KeyNotFoundException($"Permission type with id {command.PermissionTypeId} was not found");
            }

            permission.EmployeeForename = command.EmployeeForename;
            permission.EmployeeSurname = command.EmployeeSurname;
            permission.PermissionDate = DateTime.UtcNow;
            permission.PermissionType = permissionType;

            await _unitOfWork.Permissions.UpdatePermissionAsync(permission);
            await _unitOfWork.SaveChangesAsync();

            await _elasticSearch.UpdatePermissionAsync(permission);

            return permission;
        }
    }
}
