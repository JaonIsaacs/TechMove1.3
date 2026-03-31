namespace TechMove1._3.Domain.Entities
{
    public enum ContractStatus
    {
        Draft,
        Active,
        Expired,
        OnHold
    }

    public class Contract
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ContractStatus Status { get; set; }

        public string SignedFilePath { get; set; }

        public ICollection<ServiceRequest> ServiceRequests { get; set; }
    }
}
