using System.Linq;
using System.Threading.Tasks;

namespace TechMove1._3.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        Task SaveChangesAsync();
    }
}
