using Microsoft.AspNetCore.Mvc;
using UserPermissionsAdmin.Commands;
using UserPermissionsAdmin.Models;
using UserPermissionsAdmin.Queries;
using UserPermissionsAdmin.Services;

namespace UserPermissionsAdmin.Controllers;

[ApiController]
[Route("[controller]")]
public class PermissionController : ControllerBase
{   
    private readonly ILogger<PermissionController> _logger;
    private readonly IPermissionService _permissionService;

    public PermissionController(IPermissionService permissionService, ILogger<PermissionController> logger)
    {
        _logger = logger;
        _permissionService = permissionService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Permission>> GetPermission(int id)
    {
        var query = new GetPermissionQuery { PermissionId = id};
        var permission = await _permissionService.GetPermissionByIdAsync(query);

        return Ok(permission);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Permission>>> GetPermissions()
    {
        var permissions = await _permissionService.GetAllPermissionsAsync();

        return Ok(permissions);
    }

    [HttpPost]
    public async Task<ActionResult> RequestPermission([FromBody] CreatePermissionCommand command)
    {
        var permission = await _permissionService.CreatePermissionAsync(command);

        return CreatedAtAction(nameof(GetPermission), new { id = permission.Id}, permission);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> ModifyPermission(int id, [FromBody] UpdatePermissionCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("Permission ID mismatch");
        }

        await _permissionService.UpdatePermissionAsync (command);

        return NoContent(); 
    }
}
