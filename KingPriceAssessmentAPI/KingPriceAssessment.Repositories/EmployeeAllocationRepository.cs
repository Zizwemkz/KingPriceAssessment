using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Common.Repository;
using KingPriceAssessment.Data;
using KingPriceAssessment.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace KingPriceAssessment.Repositories
{
    public class EmployeeAllocationRepository : IEmployeeAllocationRepository
    {
        private readonly EmployeeDbContext EmployeeDbContext;

        public EmployeeAllocationRepository(EmployeeDbContext context)
        {
            EmployeeDbContext = context;
        }

        public async Task<IEnumerable<EmployeeAllocation>> GetAllAsync() =>
            await EmployeeDbContext.EmployeeAllocation
                .Include(a => a.Employee)
                .Include(a => a.Role)
                .Include(a => a.Department)
                .ToListAsync();

        public async Task<EmployeeAllocation> GetByIdAsync(int id) =>
            await EmployeeDbContext.EmployeeAllocation
                .Include(a => a.Employee)
                .Include(a => a.Role)
                .Include(a => a.Department)
                .FirstOrDefaultAsync(a => a.Id == id);

        public async Task AddAsync(EmployeeAllocation allocation)
        {
            EmployeeDbContext.EmployeeAllocation.Add(allocation);
            await EmployeeDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(EmployeeAllocation allocation)
        {
            EmployeeDbContext.EmployeeAllocation.Update(allocation);
            await EmployeeDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var allocation = await EmployeeDbContext.EmployeeAllocation.FindAsync(id);
            if (allocation != null)
            {
                EmployeeDbContext.EmployeeAllocation.Remove(allocation);
                await EmployeeDbContext.SaveChangesAsync();
            }
        }
    }
}
