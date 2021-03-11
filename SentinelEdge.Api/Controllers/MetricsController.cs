using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;

namespace SentinelEdge.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MetricsController : ControllerBase
    {
        private static readonly string[] _scopeRequiredByApi = new string[] { "api://fb900fd0-3383-47dc-8a34-f25643f3b69f/Firewall.Update" };
        private readonly ILogger<MetricsController> _logger;

        public MetricsController(ILogger<MetricsController> logger)
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