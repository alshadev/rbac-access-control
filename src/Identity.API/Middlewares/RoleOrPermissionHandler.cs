using System;
using Microsoft.AspNetCore.Authorization;

namespace Identity.API.Middlewares;

public class RoleOrPermissionHandler : AuthorizationHandler<RoleOrPermissionRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public RoleOrPermissionHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleOrPermissionRequirement requirement)
    {
        var endpoint = _httpContextAccessor.HttpContext.GetEndpoint();

        var attribute = endpoint?.Metadata.GetMetadata<CustomAuthorizeAttribute>();

        if (attribute == null)
            return Task.CompletedTask;

        var roles = attribute.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var permission = attribute.Permission;

        // Check roles
        if (roles.Any(context.User.IsInRole))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // Check permission
        if (context.User.HasClaim(c => c.Type == "permission" && c.Value == permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
