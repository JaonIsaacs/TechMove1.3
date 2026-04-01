using System.Threading.Tasks;
using TechMove1._3.Domain.Interfaces;
using Xunit;

namespace TechMove1._3.Tests
{
    public class CurrencyTests
    {
        [Fact]
        public async Task Convert_ShouldWork()
        {
            var fake = new FakeStrategy(20);

            var result = await fake.ConvertToZAR(10);

            Assert.Equal(200, result);
        }
    }

    public class FakeStrategy : ICurrencyStrategy
    {
        private readonly decimal _rate;

        public FakeStrategy(decimal rate)
        {
            _rate = rate;
        }

        public Task<decimal> ConvertToZAR(decimal usd)
        {
            return Task.FromResult(usd * _rate);
        }
    }
}
