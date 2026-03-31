namespace TechMove1._3.Domain.Interfaces
{
    public interface ICurrencyStrategy
    {
        Task<decimal> ConvertToZAR(decimal usd);
    }
}
