using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace SentinelEdge.Api.Models
{
    public class SentinelIpSet
    {
        public List<SentinelEntity> Ips { get; set; }

        [JsonConstructor]
        public SentinelIpSet(List<SentinelEntity> ips) => Ips = ips;

    }
}