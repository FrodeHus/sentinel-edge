using System.Text.Json;
using SentinelEdge.Api.Unifi;
using Xunit;
using FluentAssertions;
using System.Linq;
using NSubstitute;
using System.Net.Http;
using System.Net;
using System;
using Microsoft.Extensions.Logging;
using SentinelEdge.Api.Services;
using System.Threading.Tasks;

namespace SentinelEdge.Api.Tests
{
    public class UnifiTests
    {
        [Fact]
        public async Task ServiceCanRetrieveFirewallGroups()
        {
            const string json = @"{""meta"":{""rc"":""ok""},""data"":[{""_id"":""5f9c30eea170e204f9508245"",""name"":""Block IPs"",""group_type"":""address-group"",""group_members"":[""11.22.33.44""],""site_id"":""5e57c7c5a5a9925458b5b3d6""}]}";
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            var httpClient = new HttpClient(new FakeHttpMessageHandler(json, HttpStatusCode.OK)) { BaseAddress = new Uri("https://localhost") };
            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(httpClient);
            var fakeLog = Substitute.For<ILogger<ITalkToFirewall>>();
            var service = new UnifiFirewallService(new Configuration.UsgConfiguration { Url = new Uri("https://localhost") }, fakeLog, httpClientFactory);
            var actual = await service.ListFirewallGroups().ConfigureAwait(false);
            actual.Should().HaveCount(1);
            actual.Single().GroupMembers.Should().HaveCount(1);
        }

        [Fact]
        public async Task ServiceCanRetrieveFirewallRules()
        {
            const string json = @"{""meta"":{""rc"":""ok""},""data"":[{""_id"":""5f955b76a170e204f93a8e66"",""action"":""drop"",""enabled"":true,""dst_address"":"""",""dst_firewallgroup_ids"":[],""dst_networkconf_type"":""NETv4"",""icmp_typename"":"""",""ipsec"":"""",""logging"":true,""name"":""Log denied traffic"",""protocol"":""all"",""protocol_match_excepted"":false,""ruleset"":""WAN_LOCAL"",""src_firewallgroup_ids"":[],""src_address"":"""",""src_mac_address"":"""",""src_networkconf_type"":""NETv4"",""state_established"":false,""state_invalid"":true,""state_new"":true,""state_related"":true,""dst_networkconf_id"":"""",""src_networkconf_id"":"""",""rule_index"":""4000"",""site_id"":""5e57c7c5a5a9925458b5b3d6""}]}";
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            var httpClient = new HttpClient(new FakeHttpMessageHandler(json, HttpStatusCode.OK)) { BaseAddress = new Uri("https://localhost") };
            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(httpClient);
            var fakeLog = Substitute.For<ILogger<ITalkToFirewall>>();
            var service = new UnifiFirewallService(new Configuration.UsgConfiguration { Url = new Uri("https://localhost") }, fakeLog, httpClientFactory);
            var actual = await service.ListRules().ConfigureAwait(false);
            actual.Should().HaveCount(1);
        }
    }
}
