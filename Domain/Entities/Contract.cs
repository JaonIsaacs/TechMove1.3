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

        // legacy field retained for compatibility; metadata is the source of truth
        public string SignedFilePath { get; set; }

        public ICollection<ServiceRequest> ServiceRequests { get; set; }

        // new: store metadata rows (one-to-many: contract -> files)
        public ICollection<FileMetadata> Files { get; set; }
    }
}
