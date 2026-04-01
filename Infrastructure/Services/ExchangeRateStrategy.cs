using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TechMove1._3.Domain.Interfaces;

namespace TechMove1._3.Infrastructure.Services
{
    public class ExchangeRateStrategy : ICurrencyStrategy
    {
        private readonly HttpClient _http;
        private readonly IMemoryCache _cache;
        private static readonly string CacheKey = "ExchangeRates_USD";

        public ExchangeRateStrategy(HttpClient http, IMemoryCache cache)
        {
            _http = http;
            _cache = cache;
        }

        public async Task<decimal> ConvertToZAR(decimal usd)
        {
            try
            {
                var resp = await _cache.GetOrCreateAsync(CacheKey, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    // endpoint returns JSON containing "rates"
                    return await _http.GetFromJsonAsync<ExchangeResponse>("v4/latest/USD");
                });

                if (resp?.Rates == null || !resp.Rates.TryGetValue("ZAR", out var rate))
                    throw new InvalidOperationException("ZAR rate not found in response.");

                return Math.Round(usd * rate, 2);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to convert USD to ZAR", ex);
            }
        }

        private class ExchangeResponse
        {
            public string Base { get; set; }
            public Dictionary<string, decimal> Rates { get; set; }
        }
    }
}
