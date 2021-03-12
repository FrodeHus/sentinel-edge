using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SentinelEdge.Api.Unifi
{
    public class UsgResultSet<TDataType>
    {
        [JsonConstructor]
        public UsgResultSet(UsgResultMetadata meta, IEnumerable<TDataType> data) => (Meta, Data) = (meta, data);

        public UsgResultMetadata Meta { get; set; }
        public IEnumerable<TDataType> Data { get; set; }
    }

    public class UsgResultMetadata{
        [JsonPropertyName("rc")]
        public string ResultCode { get; set; }
    }
}