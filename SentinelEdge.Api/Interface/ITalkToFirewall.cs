using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SentinelEdge.Api.Models;
using SentinelEdge.Api.Unifi;

namespace SentinelEdge.Api.Services
{
    public interface ITalkToFirewall
    {
        Task<List<FirewallRule>> ListRules();
        Task<List<FirewallGroup>> ListFirewallGroups();
        Task UpdateFirewallGroup(FirewallGroup group);
        Task AddRule(FirewallRule rule);
        Task BlockIPs(List<SentinelEntity> ips);
    }
}