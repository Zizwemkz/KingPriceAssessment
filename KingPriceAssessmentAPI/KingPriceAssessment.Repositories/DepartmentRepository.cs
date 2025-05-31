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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly EmployeeDbContext _EmployeeDb;

        public DepartmentRepository(EmployeeDbContext context)
        {
            _EmployeeDb = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync() => await _EmployeeDb.Departments.ToListAsync();
        public async Task<Department> GetByIdAsync(int id) => await _EmployeeDb.Departments.FindAsync(id);
        public async Task AddAsync(Department department) { _EmployeeDb.Departments.Add(department); await _EmployeeDb.SaveChangesAsync(); }
        public async Task UpdateAsync(Department department) { _EmployeeDb.Departments.Update(department); await _EmployeeDb.SaveChangesAsync(); }
        public async Task DeleteAsync(int id)
        {
            var dpt = await _EmployeeDb.Departments.FindAsync(id);
            if (dpt != null)
            {
                _EmployeeDb.Departments.Remove(dpt);
                await _EmployeeDb.SaveChangesAsync();
            }
        }
    }
}
