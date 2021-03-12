using System.Collections.Generic;
using System.Text.Json;
using FluentAssertions;
using SentinelEdge.Api.Models;
using Xunit;

namespace SentinelEdge.Api.Tests
{
    public class SentinelTests
    {
        [Fact]
        public void WeCanDeserializeEntitiesFromSentinel()
        {
            var json = @"[
  {
    ""$id"": ""1"",
    ""Address"": ""111.222.333.444"",
    ""Type"": ""ip""
  }
]";
            var entities = JsonSerializer.Deserialize<List<SentinelEntity>>(json);
            entities.Should().HaveCount(1);
        }
    }
}