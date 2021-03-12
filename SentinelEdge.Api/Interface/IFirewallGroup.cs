using System.Collections.Generic;

namespace SentinelEdge.Api.Models
{
    public interface IFirewallGroup
    {
        string Id { get; set; }
        string SiteId { get; set; }
        string Name { get; set; }
        string GroupType { get; set; }
        IEnumerable<string> GroupMembers { get; set; }
    }
}