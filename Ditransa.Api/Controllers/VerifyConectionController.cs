using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ditransa.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class VerifyConectionController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult VerifyConection()
        {
            return Ok("Connection is OK");
        }
    }
}
