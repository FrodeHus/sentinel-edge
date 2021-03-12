using System.Text.Json.Serialization;

namespace SentinelEdge.Api.Models
{
    public class SentinelEntity
    {
        public string Address { get; set; }
        public string Type { get; set; }
        [JsonConstructor]
        public SentinelEntity(string address, string type) => (Address, Type) = (address, type);
    }
}