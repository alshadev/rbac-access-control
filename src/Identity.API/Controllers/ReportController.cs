using Identity.API.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        [Authorize(Policy = "DynamicPolicy")]
        [CustomAuthorize(Roles = "Admin,AdminReport,StaffReport", Permission = "ViewReport")]
        [HttpGet("View")]
        public IActionResult ViewReport()
        {
            return Ok("Authorized, you have one of this Roles 'Admin,AdminReport,StaffReport' or Permissions 'ViewReport'");
        }

        [Authorize(Policy = "DynamicPolicy")]
        [CustomAuthorize(Roles = "Admin,AdminReport", Permission = "SendReport")]
        [HttpPost("Send")]
        public IActionResult SendReport()
        {
            return Ok("Authorized, you have one of this Roles 'Admin,AdminReport' or Permissions 'SendReport'");
        }
    }
}
