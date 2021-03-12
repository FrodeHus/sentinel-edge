using System.Collections.Generic;
using System.Threading.Tasks;
using SentinelEdge.Api.Models;

namespace SentinelEdge.Api.Services
{
    public interface ITalkToFirewall
    {
        Task<List<IFirewallRule>> ListRules();
        Task AddRule(IFirewallRule rule);
        Task BlockIP(string ipAddress);
    }
}