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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext EmployeeDb;

        public EmployeeRepository(EmployeeDbContext context)
        {
            EmployeeDb = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync() => await EmployeeDb.Employees.ToListAsync();
        public async Task<Employee> GetByIdAsync(int id) => await EmployeeDb.Employees.FindAsync(id);
        public async Task AddAsync(Employee employee) { EmployeeDb.Employees.Add(employee); await EmployeeDb.SaveChangesAsync(); }
        public async Task UpdateAsync(Employee employee) { EmployeeDb.Employees.Update(employee); await EmployeeDb.SaveChangesAsync(); }
        public async Task DeleteAsync(int id)
        {
            var emp = await EmployeeDb.Employees.FindAsync(id);
            if (emp != null)
            {
                EmployeeDb.Employees.Remove(emp);
                await EmployeeDb.SaveChangesAsync();
            }
        }
    }
}
