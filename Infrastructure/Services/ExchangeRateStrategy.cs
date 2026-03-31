using TechMove1._3.Domain.Interfaces;
using Newtonsoft.Json;

namespace TechMove1._3.Infrastructure.Services
{
    public class ExchangeRateStrategy : ICurrencyStrategy
    {
        private readonly HttpClient _http;

        public ExchangeRateStrategy(HttpClient http)
        {
            _http = http;
        }

        public async Task<decimal> ConvertToZAR(decimal usd)
        {
            var json = await _http.GetStringAsync("https://api.exchangerate-api.com/v4/latest/USD");
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            decimal rate = data.rates.ZAR;

            return usd * rate;
        }
    }
}
