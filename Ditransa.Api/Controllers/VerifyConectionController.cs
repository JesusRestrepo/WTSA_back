using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Ditransa.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[action]")]
    [EnableRateLimiting("LoginPolicy")]
    public class VerifyConectionController : ControllerBase
    {
        private readonly ILogger<VerifyConectionController> _logger;
        public VerifyConectionController(ILogger<VerifyConectionController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult VerifyConection()
        {
            return Ok(new { dataObject = "connection is ok." });
        }
    }
}
