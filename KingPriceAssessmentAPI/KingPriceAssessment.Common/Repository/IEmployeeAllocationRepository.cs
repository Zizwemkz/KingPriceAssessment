using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Data.Tables;

namespace KingPriceAssessment.Common.Repository
{
    public interface IEmployeeAllocationRepository
    {
        public Task<IEnumerable<EmployeeAllocation>> GetAllAsync();
        Task<EmployeeAllocation> GetByIdAsync(int id);
        Task AddAsync(EmployeeAllocation allocation);
        Task UpdateAsync(EmployeeAllocation allocation);
        Task DeleteAsync(int id);
    }
}
