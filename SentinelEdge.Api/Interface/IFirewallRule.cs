using System.Collections.Generic;

namespace SentinelEdge.Api.Models
{
    public interface IFirewallRule
    {
        string Id { get; set; }
        string SiteId { get; set; }
        string Name { get; set; }
        string Action { get; set; }
        bool Enabled { get; set; }
        string RuleSet { get; set; }
        IEnumerable<string> DestinationFirewallGroups { get; set; }
        IEnumerable<string> SourceFirewallGroups { get; set; }
        int RuleIndex { get; set; }
        string Protocol { get; set; }
        string SourceAddress { get; set; }
        string SourceMacAddress { get; set; }
        string SourceNetworkType { get; set; }
        string DestinationAddress { get; set; }
        bool Logging { get; set; }
        bool StateEstablished { get; set; }
        bool StateInvalid { get; set; }
        bool StateNew { get; set; }
        bool StateRelated { get; set; }
    }
}