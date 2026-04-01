using System.Linq;
using System.Threading.Tasks;
using TechMove1._3.Domain.Entities;

namespace TechMove1._3.Domain.Interfaces
{
    public interface IContractRepository : IRepository<Contract>
    {
        IQueryable<Contract> QueryWithClient();
        Task<Contract> FindAsync(int id);
    }
}
