using Microsoft.EntityFrameworkCore;
using TechMove1._3.Domain.Entities;
using TechMove1._3.Infrastructure.Data;

namespace TechMove1._3.Application.Services
{
    public class ContractService
    {
        private readonly AppDbContext _context;

        public ContractService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Contract>> Search(
            DateTime? start, DateTime? end, ContractStatus? status)
        {
            var query = _context.Contracts.AsQueryable();

            if (start.HasValue)
                query = query.Where(c => c.StartDate >= start);

            if (end.HasValue)
                query = query.Where(c => c.EndDate <= end);

            if (status.HasValue)
                query = query.Where(c => c.Status == status);

            return await query.ToListAsync();
        }
    }
}
