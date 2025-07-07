using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ditransa.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
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
            return Ok(new { dataObject = "Hola Mundo" });
        }
    }
}
