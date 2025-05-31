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
        private readonly EmployeeDbContext _employeeDbContext;

        public DepartmentRepository(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            try
            {
                return await _employeeDbContext.Departments.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving departments.", ex);
            }
        }
        public async Task<Department> GetByIdAsync(int id)
        {
            try
            {
                var department = await _employeeDbContext.Departments.FindAsync(id);
                return department;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the department.", ex);
            }
        }
        public async Task AddAsync(Department department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            try
            {
                await _employeeDbContext.Departments.AddAsync(department);
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the department.", ex);
            }
        }
        public async Task UpdateAsync(Department department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            try
            {
                _employeeDbContext.Departments.Update(department);
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the department.", ex);
            }
        }
        public async Task DeleteAsync(int id)
        {
            try
            {
                var dpt = await _employeeDbContext.Departments.FindAsync(id);
                _employeeDbContext.Departments.Remove(dpt);
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the department.", ex);
            }
        }
    }
}
