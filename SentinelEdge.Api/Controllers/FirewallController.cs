using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using SentinelEdge.Api.Models;
using SentinelEdge.Api.Services;

namespace SentinelEdge.Api.Controllers
{
    [Authorize(Roles = "Task.Firewall.Read,Task.Firewall.Update")]
    [ApiController]
    [Route("[controller]")]
    public class FirewallController : ControllerBase
    {
        private static readonly string[] _scopeRequiredByApi = new string[] { "api://fb900fd0-3383-47dc-8a34-f25643f3b69f/Firewall.Read", "Firewall.Read" };
        private readonly ILogger<FirewallController> _logger;
        private readonly ITalkToFirewall _firewall;

        public FirewallController(ILogger<FirewallController> logger, ITalkToFirewall firewall)
        {
            _logger = logger;
            _firewall = firewall;
        }
        [HttpGet("rule")]
        public async Task<ActionResult<IEnumerable<IFirewallRule>>> GetRules()
        {
            // HttpContext.VerifyUserHasAnyAcceptedScope(_scopeRequiredByApi);
            var rules = await _firewall.ListRules().ConfigureAwait(false);
            return Ok(rules);
        }

        [HttpGet("group")]
        public async Task<ActionResult<IEnumerable<IFirewallGroup>>> GetGroups()
        {
            // HttpContext.VerifyUserHasAnyAcceptedScope(_scopeRequiredByApi);
            var groups = await _firewall.ListFirewallGroups().ConfigureAwait(false);
            return Ok(groups);
        }

        [Authorize(Roles = "Task.Firewall.Update")]
        [HttpPost("block")]
        public IActionResult BlockIPs([FromBody] JsonElement data)
        {

            var entities = JsonSerializer.Deserialize<List<SentinelEntity>>(data.GetProperty("Ips").GetRawText());

            foreach (var entity in entities)
            {
                _logger.LogInformation($"-> blocking {entity.Address}");
            }

            return Ok();
        }
    }
}