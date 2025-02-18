using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;
using UserPermissionsAdmin.Data;
using UserPermissionsAdmin.Models;

namespace UserPermissionAdminTest
{
    [TestClass ]
    public class PermissionControllerIntegrationTest
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;
        private Permission initialPermission;
        private ApplicationDbContext dbContext;

        public PermissionControllerIntegrationTest()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            _client = _factory.CreateClient();

            initialPermission = new Permission
            {
                EmployeeForename = "Guillermo",
                EmployeeSurname = "Ferreira",
                PermissionDate = DateTime.UtcNow,
                PermissionType = new PermissionType { Id = 1, Description = "Read" }
            };

            var scope = _factory.Services.CreateScope();
            dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();//_factory.Services.GetRequiredService<ApplicationDbContext>();
        }

        [TestInitialize]
        public async Task Setup()
        {            
            dbContext.Permissions.Add(initialPermission);
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task GetPermission_ReturnsPermission_WhenPermissionExists()
        {
            // Arrange
            var permissionId = 1;

            // Act
            var response = await _client.GetAsync($"api/permission/{permissionId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var permission = JsonConvert.DeserializeObject<Permission>(responseContent);

            Assert.IsNotNull(permission);
            Assert.AreEqual(permissionId, permission.Id);
            Assert.AreEqual("Guillermo", permission.EmployeeForename);
        }

        [TestMethod]
        public async Task CreatePermission_ShouldReturnCreatedStatus_WhenPermissionIsValid()
        {
            // Arrange
            var newPermission = new Permission
            {
                EmployeeForename = "Sergio",
                EmployeeSurname = "Garcia",
                PermissionType = new PermissionType { Id = 1, Description = "Read" },
                PermissionDate = DateTime.UtcNow
            };

            var content = new StringContent(JsonConvert.SerializeObject(newPermission), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("api/permission", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var createdPermission = JsonConvert.DeserializeObject<Permission>(responseContent);

            createdPermission.Should().NotBeNull();
            newPermission.EmployeeSurname.Should().Be(createdPermission.EmployeeSurname);
            newPermission.EmployeeForename.Should().Be(createdPermission.EmployeeForename);
            newPermission.PermissionDate.Should().Be(createdPermission.PermissionDate);
            newPermission.PermissionType.Id.Should().Be(createdPermission.PermissionType.Id);
            newPermission.PermissionType.Description.Should().Be(createdPermission.PermissionType.Description);
        }

        [TestMethod]
        public async Task UpdatePermission_ShouldReturnNoContentStatus_WhenPermissionIsValidAndPermissionExists()
        {
            var updatedPermission = new Permission
            {
                Id = initialPermission.Id,
                EmployeeForename = "Updated Guillermo",
                EmployeeSurname = initialPermission.EmployeeSurname,
                PermissionType = initialPermission.PermissionType,
                PermissionDate = DateTime.UtcNow
            };

            var updateContent = new StringContent(JsonConvert.SerializeObject(updatedPermission), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"api/permission/{updatedPermission.Id}", updateContent);

            // Assert
            response.EnsureSuccessStatusCode();

            var permissionFromDb = await dbContext.Permissions.FindAsync(initialPermission.Id);
            permissionFromDb.Should().NotBeNull();
            permissionFromDb.EmployeeForename.Should().Be(updatedPermission.EmployeeForename);
            permissionFromDb.EmployeeSurname.Should().Be(updatedPermission.EmployeeForename);
            permissionFromDb.PermissionType.Id.Should().Be(updatedPermission.PermissionType.Id);
            permissionFromDb.PermissionType.Description.Should().Be(updatedPermission.PermissionType.Description);
            permissionFromDb.PermissionDate.Should().Be(updatedPermission.PermissionDate);
        }
    }
}
