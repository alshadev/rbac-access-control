using System;

namespace Identity.API.Requests;

public class CreateRoleRequest
{
    public string Name { get; set; }
    public List<Guid> PermissionIds { get; set; }
}
