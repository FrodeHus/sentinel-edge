using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;

namespace SentinelEdge.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FirewallController : ControllerBase
    {
        private static readonly string[] _scopeRequiredByApi = new string[] { "api://fb900fd0-3383-47dc-8a34-f25643f3b69f/Metrics.Read" };
        private readonly ILogger<FirewallController> _logger;

        public FirewallController(ILogger<FirewallController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Get()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(_scopeRequiredByApi);
            return Ok();
        }
    }
}