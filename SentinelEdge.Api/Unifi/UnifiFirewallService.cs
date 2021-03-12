using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SentinelEdge.Api.Configuration;
using SentinelEdge.Api.Models;
using SentinelEdge.Api.Services;
using SentinelEdge.Api.Unifi;

namespace SentinelEdge.Api.Unifi
{
    /// <summary>
    /// Service for communcating with Unifi USG firewall
    /// </summary>
    public class UnifiFirewallService : ITalkToFirewall
    {
        private readonly UsgConfiguration _config;
        private readonly ILogger<ITalkToFirewall> _logger;
        private readonly HttpClient _client;

        public UnifiFirewallService(UsgConfiguration config, ILogger<ITalkToFirewall> logger, IHttpClientFactory clientFactory)
        {
            _config = config;
            _logger = logger;
            _client = clientFactory.CreateClient();
        }

        private async Task Authenticate()
        {
            var auth = new AuthenticationPayload { Username = _config.Username, Password = _config.Password };
            var result = await _client.PostAsJsonAsync(new Uri($"{_config.Url}/api/login"), auth).ConfigureAwait(false);
            if (!result.IsSuccessStatusCode)
            {
                var reason = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                _logger.LogError("Failed to authenticate with firewall: " + reason);
                result.EnsureSuccessStatusCode();
            }
        }

        public Task AddRule(IFirewallRule rule)
        {
            throw new System.NotImplementedException();
        }

        public Task BlockIP(string ipAddress)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<IFirewallRule>> ListRules()
        {
            await Authenticate().ConfigureAwait(false);
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var result = await _client.GetFromJsonAsync<UsgResultSet<IFirewallRule>>(new Uri($"{_config.Url}/api/s/default/rest/firewallrule"), options).ConfigureAwait(false);
            return result.Data.ToList();

        }
    }
}