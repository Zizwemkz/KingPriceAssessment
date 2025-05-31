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

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            try
            {
                return await _EmployeeDb.Departments.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving departments.", ex);
            }
        }
        public async Task<Department> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Department ID must be greater than 0.", nameof(id));

            try
            {
                var department = await _EmployeeDb.Departments.FindAsync(id);
                if (department == null)
                    throw new KeyNotFoundException($"No department found with ID {id}.");
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
                if (await _EmployeeDb.Departments.AnyAsync(d => d.DepartmentName.ToLower() == department.DepartmentName.ToLower()))
                    throw new InvalidOperationException($"A department with the name '{department.DepartmentName}' already exists.");

                await _EmployeeDb.Departments.AddAsync(department);
                await _EmployeeDb.SaveChangesAsync();
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
                var existingDepartment = await _EmployeeDb.Departments.FindAsync(department.Id);
                if (existingDepartment == null)
                    throw new KeyNotFoundException($"No department found with ID {department.Id}.");

                if (await _EmployeeDb.Departments.AnyAsync(d => d.DepartmentName.ToLower() == department.DepartmentName.ToLower() && d.Id != department.Id))
                    throw new InvalidOperationException($"A department with the name '{department.DepartmentName}' already exists.");

                _EmployeeDb.Entry(existingDepartment).CurrentValues.SetValues(department);
                await _EmployeeDb.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the department.", ex);
            }
        }
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Department ID must be greater than 0.", nameof(id));

            try
            {
                var dpt = await _EmployeeDb.Departments.FindAsync(id);
                if (dpt == null)
                    throw new KeyNotFoundException($"No department found with ID {id}.");

                _EmployeeDb.Departments.Remove(dpt);
                await _EmployeeDb.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the department.", ex);
            }
        }
    }
}
