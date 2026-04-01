namespace TechMove1._3.Domain.Entities
{
    public enum ServiceStatus
    {
        Pending,
        Completed
    }

    public class ServiceRequest
    {
        public int Id { get; set; }

        public int ContractId { get; set; }
        public Contract Contract { get; set; }

        public string Description { get; set; }
        public decimal CostZAR { get; set; }

        // Added to match DbContext precision configuration and support USD storage if needed.
        public decimal CostUSD { get; set; }

        public ServiceStatus Status { get; set; }
    }
}
