using System.Collections.Generic;
using System.Text.Json.Serialization;
using SentinelEdge.Api.Models;

namespace SentinelEdge.Api.Models
{
    public class FirewallGroup
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        [JsonPropertyName("site_id")]
        public string SiteId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("group_type")]
        public string GroupType { get; set; }
        [JsonPropertyName("group_members")]
        public IEnumerable<string> GroupMembers { get; set; }
    }
}