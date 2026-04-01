using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var query = _context.Contracts.Include(c => c.Client).AsQueryable();

            if (start.HasValue)
            {
                var s = start.Value.Date;
                query = query.Where(c => c.StartDate.Date >= s);
            }

            if (end.HasValue)
            {
                var e = end.Value.Date;
                query = query.Where(c => c.EndDate.Date <= e);
            }                       {
                var e = end.Value.Date;
                query = query.Where(c => c.EndDate.Date <= e);
            }

            if (status.HasValue)
                query = query.Where(c => c.Status == status);

            return await query.ToListAsync();
        }
    }
}
