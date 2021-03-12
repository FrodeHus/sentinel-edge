using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SentinelEdge.Api.Models;

namespace SentinelEdge.Api.Models
{
    public class FirewallRule
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        [JsonPropertyName("site_id")]
        public string SiteId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("action")]
        public string Action { get; set; }
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
        [JsonPropertyName("ruleset")]
        public string RuleSet { get; set; }
        [JsonPropertyName("dst_firewallgroup_ids")]
        public IEnumerable<string> DestinationFirewallGroups { get; set; }
        [JsonPropertyName("src_firewallgroup_ids")]
        public IEnumerable<string> SourceFirewallGroups { get; set; }
        [JsonPropertyName("rule_index")]
        public int RuleIndex { get; set; }
        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }
        [JsonPropertyName("src_address")]
        public string SourceAddress { get; set; }
        [JsonPropertyName("src_mac_address")]
        public string SourceMacAddress { get; set; }
        [JsonPropertyName("src_networkconf_type")]
        public string SourceNetworkType { get; set; }
        [JsonPropertyName("dst_address")]
        public string DestinationAddress { get; set; }
        [JsonPropertyName("logging")]
        public bool Logging { get; set; }
        [JsonPropertyName("state_established")]
        public bool StateEstablished { get; set; }
        [JsonPropertyName("state_invalid")]
        public bool StateInvalid { get; set; }
        [JsonPropertyName("state_new")]
        public bool StateNew { get; set; }
        [JsonPropertyName("state_related")]
        public bool StateRelated { get; set; }
    }
}