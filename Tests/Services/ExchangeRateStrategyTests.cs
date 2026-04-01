using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Xunit;
using TechMove1._3.Infrastructure.Services;

namespace TechMove1._3.Tests.Services
{
    public class ExchangeRateStrategyTests
    {
        private class FakeHandler : DelegatingHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var json = @"{ ""base"": ""USD"", ""rates"": { ""ZAR"": 18.5 } }";
                var msg = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
                };
                return Task.FromResult(msg);
            }
        }

        [Fact]
        public async Task ConvertToZAR_ReturnsExpectedValue()
        {
            var handler = new FakeHandler();
            var client = new HttpClient(handler) { BaseAddress = new System.Uri("https://api.exchangerate-api.com/") };

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var strategy = new ExchangeRateStrategy(client, memoryCache);

            var result = await strategy.ConvertToZAR(10m);

            Assert.Equal(185.00m, result);
        }
    }
}
