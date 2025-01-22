using System;
using Microsoft.AspNetCore.Authorization;

namespace Identity.API.Middlewares;

public class RoleOrPermissionRequirement : IAuthorizationRequirement
{
    public IEnumerable<string> Roles { get; }
    public string Permission { get; }

    public RoleOrPermissionRequirement(IEnumerable<string> roles, string permission)
    {
        Roles = roles;
        Permission = permission;
    }
}