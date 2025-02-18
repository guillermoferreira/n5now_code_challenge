using Moq;
using UserPermissionsAdmin.Commands;
using UserPermissionsAdmin.Data;
using UserPermissionsAdmin.Models;
using UserPermissionsAdmin.Queries;
using UserPermissionsAdmin.Services;

namespace UserPermissionAdminTest
{
    [TestClass]
    public sealed class PermissionServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IPermissionService _permissionService;
        private Mock<IElasticSearchService> _mockElasticService;

        [TestInitialize]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockElasticService = new Mock<IElasticSearchService>();
            _permissionService = new PermissionService(_mockUnitOfWork.Object, _mockElasticService.Object);            

            _mockElasticService.Setup(e => e.IndexPermissionAsync(It.IsAny<Permission>())).Returns(Task.CompletedTask);
            _mockElasticService.Setup(e => e.UpdatePermissionAsync(It.IsAny<Permission>())).Returns(Task.CompletedTask);
            _mockElasticService.Setup(e => e.GetPermissionByIdAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<Permission>());
        }

        [TestMethod]
        public async Task GetPermissionByIdAsync_ShouldReturnPermission_WhenPermissionExists()
        {
            // Arrange
            var permissionId = 1;
            var permissionTypeId = 1;
            var permissionType = new PermissionType { Id = permissionTypeId, Description = "Read" };
            var permission = new Permission { Id = permissionId, EmployeeForename = "User Forename", EmployeeSurname = "User Surname", PermissionDate = DateTime.UtcNow, PermissionType = permissionType};

            _mockUnitOfWork.Setup(r => r.Permissions.GetPermissionByIdAsync(permissionId)).ReturnsAsync(permission);

            // Act
            var result = await _permissionService.GetPermissionByIdAsync(new GetPermissionQuery { PermissionId = permissionId });

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(permissionId, result.Id);
            Assert.AreEqual("User Forename", result.EmployeeForename);
            Assert.AreEqual("User Surname", result.EmployeeSurname);
            Assert.AreEqual(permissionTypeId, result.PermissionType.Id);
        }

        [TestMethod]
        public async Task GetPermissionByIdAsync_ShouldThrowKeyNotFoundException_WhenPermissionNotExists()
        {
            // Arrange
            var nonExistentPermissionId = 4;
            var permissionTypeId = 1;
            var permissionType = new PermissionType { Id = permissionTypeId, Description = "Read" };
            var getPermissionQuery = new GetPermissionQuery { PermissionId = nonExistentPermissionId };

            _mockUnitOfWork.Setup(r => r.PermissionTypes.GetPermissionTypeByIdAsync(permissionTypeId)).ReturnsAsync(permissionType);
            _mockUnitOfWork.Setup(r => r.Permissions.GetPermissionByIdAsync(nonExistentPermissionId)).ReturnsAsync((Permission?)null);

            // Act
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await _permissionService.GetPermissionByIdAsync(getPermissionQuery));
        }

        [TestMethod]
        public async Task UpdatePermissionAsync_ShouldUpdatePermission_WhenPermissionExists()
        {
            // Arrange
            var permissionType = new PermissionType { Id = 1, Description = "Read" };
            var existingPermission = new Permission
            {
                Id = 1,
                EmployeeForename = "User Forename",
                EmployeeSurname = "User Surname",
                PermissionType = permissionType
            };

            var updatePermissionCommand = new UpdatePermissionCommand
            {
                Id = 1,
                EmployeeForename = "Updated User Forename",
                EmployeeSurname = "Updated User Surname",
                PermissionTypeId = 1
            };

            _mockUnitOfWork.Setup(r => r.PermissionTypes.GetPermissionTypeByIdAsync(updatePermissionCommand.PermissionTypeId))
                                    .ReturnsAsync(permissionType);

            _mockUnitOfWork.Setup(r => r.Permissions.GetPermissionByIdAsync(updatePermissionCommand.Id))
                                    .ReturnsAsync(existingPermission);

            _mockUnitOfWork.Setup(r => r.SaveChangesAsync()).ReturnsAsync(It.IsAny<int>());

            // Act
            var result = await _permissionService.UpdatePermissionAsync(updatePermissionCommand);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, updatePermissionCommand.Id);
            Assert.AreEqual(result.EmployeeForename, updatePermissionCommand.EmployeeForename);
            Assert.AreEqual(result.EmployeeSurname, updatePermissionCommand.EmployeeSurname);
            Assert.AreEqual(result.PermissionType.Id, updatePermissionCommand.PermissionTypeId);
        }

        [TestMethod]
        public async Task UpdatePermissionAsync_ShouldThrowKeyNotFoundException_WhenPermissionNotExists()
        {
            // Arrange
            var permissionType = new PermissionType { Id = 1, Description = "Read" };
            var nonExistentPermissionId = 4;
            var updatePermissionCommand = new UpdatePermissionCommand
            {
                Id = nonExistentPermissionId,
                EmployeeForename = "Updated Forename",
                EmployeeSurname = "Updated Surname",
                PermissionTypeId = 1
            };

            _mockUnitOfWork.Setup(r => r.PermissionTypes.GetPermissionTypeByIdAsync(updatePermissionCommand.PermissionTypeId))
                        .ReturnsAsync(permissionType);

            _mockUnitOfWork.Setup(r => r.Permissions.GetPermissionByIdAsync(nonExistentPermissionId)).ReturnsAsync((Permission?)null);
           
            // Act/Assert
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await _permissionService.UpdatePermissionAsync(updatePermissionCommand));
        }


        [TestMethod]
        public async Task CreatePermissionAsync_ShouldCreateNewPermission()
        {
            // Arrange
            var newPermissionId = 1;
            var permissionType = new PermissionType { Id = 1, Description = "Read" };
            var createPermissionCommand = new CreatePermissionCommand
            {
                EmployeeForename = "User Forename",
                EmployeeSurname = "User Surname",
                PermissionTypeId = 1
            };

            _mockUnitOfWork.Setup(r => r.PermissionTypes.GetPermissionTypeByIdAsync(createPermissionCommand.PermissionTypeId))
                                    .ReturnsAsync(permissionType);

            _mockUnitOfWork.Setup(r => r.Permissions.AddPermissionAsync(It.IsAny<Permission>()));

            _mockUnitOfWork.Setup(r => r.SaveChangesAsync()).ReturnsAsync(newPermissionId);

            // Act
            var result = await _permissionService.CreatePermissionAsync(createPermissionCommand);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(result.Id, 0);
            Assert.AreEqual(createPermissionCommand.EmployeeForename, result.EmployeeForename);
            Assert.AreEqual(createPermissionCommand.EmployeeSurname, result.EmployeeSurname);
            Assert.AreEqual(createPermissionCommand.PermissionTypeId, result.PermissionType.Id);
        }
    }
}
