using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SentinelEdge.Api.Models;
using SentinelEdge.Api.Services;

namespace SentinelEdge.Api.Unifi
{
    /// <summary>
    /// Service for communcating with Unifi USG firewall
    /// </summary>
    public class UnifiFirewallService : ITalkToFirewall
    {
        private readonly UsgConfiguration _config;
        private readonly ILogger<ITalkToFirewall> _logger;
        private readonly TelemetryClient _telemetryClient;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient _client;

        public UnifiFirewallService(
            IOptions<UsgConfiguration> config,
            ILogger<ITalkToFirewall> logger,
            IHttpClientFactory clientFactory,
            TelemetryClient telemetryClient,
            IHttpContextAccessor contextAccessor)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            _config = config.Value;
            _logger = logger;
            _telemetryClient = telemetryClient;
            _contextAccessor = contextAccessor;
            _client = clientFactory.CreateClient();
        }

        private async Task Authenticate()
        {
            var auth = new AuthenticationPayload { Username = _config.Username, Password = _config.Password };
            var result = await _client.PostAsJsonAsync(new Uri($"{_config.Url}api/login"), auth).ConfigureAwait(false);
            if (!result.IsSuccessStatusCode)
            {
                var reason = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                _logger.LogError("Failed to authenticate with firewall: " + reason);
                result.EnsureSuccessStatusCode();
            }
        }

        public Task AddRule(FirewallRule rule)
        {
            throw new System.NotImplementedException();
        }

        public async Task BlockIPs(List<SentinelEntity> ips)
        {
            if (ips == null)
            {
                throw new ArgumentNullException(nameof(ips));
            }

            var firewallGroups = await ListFirewallGroups().ConfigureAwait(false);
            var group = firewallGroups.Single(g => g.Name == "Block IPs");
            var previousMemberCount = group.GroupMembers.Count();
            foreach (var entity in ips.Where(e => !group.GroupMembers.Any(m => m == e.Address)))
            {
                group.GroupMembers = group.GroupMembers.Append(entity.Address);
            }

            await UpdateFirewallGroup(group).ConfigureAwait(false);

            _telemetryClient.TrackEvent(
                "FIREWALL_UPDATE",
                new Dictionary<string, string>{
                    {"User", _contextAccessor.HttpContext.User.Identity.Name},
                    {"Roles", _contextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).Aggregate(string.Empty, (current, next) => current + next.Value + ",")}
                }, new Dictionary<string, double>{
                    {"IncomingIpCount", ips.Count},
                    {"Blocked", group.GroupMembers.Count() - previousMemberCount}
                }
            );

        }

        public async Task<List<FirewallRule>> ListRules()
        {
            await Authenticate().ConfigureAwait(false);
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var result = await _client.GetFromJsonAsync<UsgResultSet<FirewallRule>>(new Uri($"{_config.Url}api/s/{_config.SiteName}/rest/firewallrule"), options).ConfigureAwait(false);
            return result.Data.ToList();
        }

        public async Task<List<FirewallGroup>> ListFirewallGroups()
        {
            await Authenticate().ConfigureAwait(false);
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var result = await _client.GetFromJsonAsync<UsgResultSet<FirewallGroup>>(new Uri($"{_config.Url}api/s/{_config.SiteName}/rest/firewallgroup"), options).ConfigureAwait(false);
            return result.Data.ToList();
        }

        public async Task UpdateFirewallGroup(FirewallGroup group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            await Authenticate().ConfigureAwait(false);
            var result = await _client.PutAsJsonAsync(new Uri($"{_config.Url}api/s/{_config.SiteName}/rest/firewallgroup/{group.Id}"), group).ConfigureAwait(false);
            if (!result.IsSuccessStatusCode)
            {
                var reason = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                _logger.LogError($"Failed updating firewall group {group.Name}: {reason}");
                result.EnsureSuccessStatusCode();
            }
        }
    }
}