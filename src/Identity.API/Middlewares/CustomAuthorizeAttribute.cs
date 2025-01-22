
using Microsoft.AspNetCore.Authorization;

namespace Identity.API.Middlewares;

public class CustomAuthorizeAttribute : AuthorizeAttribute
{
    public string Roles { get; set; } = string.Empty;
    public string Permission { get; set; } = string.Empty;
}