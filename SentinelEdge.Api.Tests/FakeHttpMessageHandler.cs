using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SentinelEdge.Api.Tests
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _fakeData;
        private readonly HttpStatusCode _resultCode;
        public FakeHttpMessageHandler(string fakeData, HttpStatusCode resultCode)
        {
            _fakeData = fakeData;
            _resultCode = resultCode;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = _resultCode,
                Content = new StringContent(_fakeData)
            });
        }
    }
}