using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SentinelEdge.Api.Models;
using SentinelEdge.Api.Services;
using Microsoft.AspNetCore.Http;

namespace SentinelEdge.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FirewallController : ControllerBase
    {
        private readonly ILogger<FirewallController> _logger;
        private readonly ITalkToFirewall _firewall;

        public FirewallController(ILogger<FirewallController> logger, ITalkToFirewall firewall)
        {
            _logger = logger;
            _firewall = firewall;
        }

        [HttpGet("rule")]
        public async Task<ActionResult<IEnumerable<FirewallRule>>> GetRules()
        {
            var rules = await _firewall.ListRules().ConfigureAwait(false);
            return Ok(rules);
        }

        [HttpGet("group")]
        public async Task<ActionResult<IEnumerable<FirewallGroup>>> GetGroups()
        {
            var groups = await _firewall.ListFirewallGroups().ConfigureAwait(false);
            return Ok(groups);
        }

        [Authorize(Policy = "FirewallAdmin")]
        [HttpPost("block")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> BlockIPs([FromBody] SentinelIpSet ipSet)
        {
            if (ipSet == null)
            {
                return BadRequest("Empty IP set");
            }
            await _firewall.BlockIPs(ipSet.Ips).ConfigureAwait(false);
            return Ok();
        }
    }
}