using Identity.API.Entities;
using Identity.API.Infrastructure;
using Identity.API.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IdentityContext _context;

        public IdentityController(IdentityContext context)
        {
            _context = context;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.Select(x => new 
            {
                x.Id,
                x.Username,
                x.Password, //in production, this password should not returned in this api
                Roles = x.UserRoles.Select(ur => new 
                { 
                    ur.Role.Id,
                    ur.Role.Name,
                    Permissions = ur.Role.RolePermissions.Select(rp => new 
                    {
                        rp.Permission.Id,
                        rp.Permission.Name
                    })
                })
            }).ToListAsync();

            return Ok(users);
        }

        [HttpPost("Users")]
        public async Task<IActionResult> CreateUsers([FromBody] CreateUserRequest request)
        {
            if (await _context.Users.AnyAsync(x => x.Username == request.Username))
            {
                return BadRequest("Username already exist.");
            }

            var roles = await _context.Roles.Where(x => request.RoleIds.Contains(x.Id)).ToListAsync();

            if (!roles.Any())
            {
                return BadRequest("Please choose valid role id(s) for this user.");
            }

            var newUser = new User();
            newUser.Username = request.Username;
            newUser.Password = request.Password;
            newUser.UserRoles = roles.Select(x => new UserRole()
            {
                Role = x,
                User = newUser,
            }).ToList();

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok($"new user with username '{newUser.Username}' has been created.");
        }

        [HttpGet("Roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles.Select(x => new 
            {
                x.Id,
                x.Name,
                Permissions = x.RolePermissions.Select(rp => new 
                { 
                    rp.Permission.Id,
                    rp.Permission.Name
                })
            }).ToListAsync();

            return Ok(roles);
        }

        [HttpPost("Roles")]
        public async Task<IActionResult> CreateRoles([FromBody] CreateRoleRequest request)
        {
            if (await _context.Roles.AnyAsync(x => x.Name == request.Name))
            {
                return BadRequest("Role name already exist.");
            }

            var permissions = await _context.Permissions.Where(x => request.PermissionIds.Contains(x.Id)).ToListAsync();

            if (!permissions.Any())
            {
                return BadRequest("Please choose valid permission id(s) for this role.");
            }

            var newRole = new Role();
            newRole.Name = request.Name;
            newRole.RolePermissions = permissions.Select(x => new RolePermission()
            {
                Role = newRole,
                Permission = x,
            }).ToList();

            await _context.Roles.AddAsync(newRole);
            await _context.SaveChangesAsync();

            return Ok($"new role with name '{newRole.Name}' has been created.");
        }

        [HttpGet("Permissions")]
        public async Task<IActionResult> GetPermissions()
        {
            var permissions = await _context.Permissions.Select(x => new 
            {
                x.Id,
                x.Name
            }).ToListAsync();

            return Ok(permissions);
        }

        [HttpPost("Permissions")]
        public async Task<IActionResult> CreatePermissions([FromBody] CreatePermissionRequest request)
        {
            if (await _context.Permissions.AnyAsync(x => x.Name == request.Name))
            {
                return BadRequest("Permission name already exist.");
            }

            var newPermission = new Permission();
            newPermission.Name = request.Name;

            await _context.Permissions.AddAsync(newPermission);
            await _context.SaveChangesAsync();

            return Ok($"new permission with name '{newPermission.Name}' has been created.");
        }
    }
}
