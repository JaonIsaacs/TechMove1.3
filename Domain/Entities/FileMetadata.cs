using System;

namespace TechMove1._3.Domain.Entities
{
    public class FileMetadata
    {
        public int Id { get; set; }

        public int ContractId { get; set; }
        public Contract Contract { get; set; }

        public string FileName { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
