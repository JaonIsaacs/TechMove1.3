using System.Diagnostics.Contracts;

namespace TechMove1._3.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }

        public ICollection<Contract> Contracts { get; set; }
    }
}
