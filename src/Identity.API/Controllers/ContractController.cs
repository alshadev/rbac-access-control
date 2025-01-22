using Identity.API.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        [Authorize(Policy = "DynamicPolicy")]
        [CustomAuthorize(Roles = "Admin,AdminContract", Permission = "ViewContract")]
        [HttpGet("View")]
        public IActionResult ViewContract()
        {
            return Ok("Authorized, you have one of this Roles 'Admin,AdminContract' or Permissions 'ViewContract'");
        }

        [Authorize(Policy = "DynamicPolicy")]
        [CustomAuthorize(Roles = "Admin,AdminContract", Permission = "CreateContract")]
        [HttpPost("Create")]
        public IActionResult CreateContract()
        {
            return Ok("Authorized, you have one of this Roles 'Admin,AdminContract' or Permissions 'CreateContract'");
        }

        [Authorize(Policy = "DynamicPolicy")]
        [CustomAuthorize(Roles = "Admin,AdminContract", Permission = "UpdateContract")]
        [HttpPut("Update")]
        public IActionResult UpdateContract()
        {
            return Ok("Authorized, you have one of this Roles 'Admin,AdminContract' or Permissions 'UpdateContract'");
        }

        [Authorize(Policy = "DynamicPolicy")]
        [CustomAuthorize(Roles = "Admin,AdminContract", Permission = "DeleteContract")]
        [HttpDelete("Delete")]
        public IActionResult DeleteContract()
        {
            return Ok("Authorized, you have one of this Roles 'Admin,AdminContract' or Permissions 'DeleteContract'");
        }
    }
}
