using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using SentinelEdge.Api.Services;

namespace SentinelEdge.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FirewallController : ControllerBase
    {
        private static readonly string[] _scopeRequiredByApi = new string[] { "api://fb900fd0-3383-47dc-8a34-f25643f3b69f/Firewall.Read" };
        private readonly ILogger<FirewallController> _logger;
        private readonly ITalkToFirewall _firewall;

        public FirewallController(ILogger<FirewallController> logger, ITalkToFirewall firewall)
        {
            _logger = logger;
            _firewall = firewall;
        }
        [HttpGet]
        public IActionResult Get()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(_scopeRequiredByApi);
            return Ok();
        }
    }
}